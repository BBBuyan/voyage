using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using Microsoft.Extensions.Logging;
using VoyageManager.Application.Abstractions;
using VoyageManager.Conventions.Agents;
using VoyageManager.Domain.Enums;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Agents;

public class WorkerService : IWorkerService
{
    private readonly IWorkerRespository _workerRespository;
    private readonly IWorkerPasswordHasher _workerPasswordHasher;
    private readonly IWorkerTokenProvider _workerTokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<WorkerService> _logger;

    public WorkerService(
        IWorkerRespository workerRepository,
        IWorkerPasswordHasher passwordHasher,
        IWorkerTokenProvider tokenProvider,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<WorkerService> logger
        )
    {
        _workerRespository = workerRepository;
        _workerPasswordHasher = passwordHasher;
        _workerTokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ErrorOr<CheckInResponse>> CheckInAsync(Guid workerId, CancellationToken ct)
    {
        Worker? worker = await _workerRespository.GetWorkerAsync(workerId, ct);
        if (worker is null)
            return Error.NotFound(description: "Worker not found.");

        if (!worker.IsEnabled)
        {
            return new CheckInResponse()
            {
                Shutdown = true,
            };
        }

        throw new NotImplementedException();
    }

    public async Task<ErrorOr<Guid>> EnrollAsync(EnrollRequest request, CancellationToken ct)
    {
        // Stage 1, validation
        Tenant? tenant = await _workerRespository
            .GetTenantById(request.TenantId, ct);

        if (tenant is null)
            return Error.NotFound(description: "Tenant not found.");

        if (tenant.EnrollmentSecret != request.EnrollmentSecret)
            return Error.Unauthorized(description: "Invalid EnrollmentToken.");

        if (tenant.AvailableLicenses == 0)
            return Error.Forbidden(description: "No licenses are available.");

        // Stage 2, creation
        string hashedPassword = _workerPasswordHasher.HashPassword(request.Password);
        Guid agentId = await _workerRespository
            .RegisterWorkerAsync(request.Name, request.HardwareId, hashedPassword, tenant.Id, ct);

        return agentId;
    }

    public async Task<ErrorOr<TokenResult>> GetTokenAsync(TokenRequest request, CancellationToken ct)
    {
        Worker? worker = await _workerRespository
            .GetWorkerAsync(request.AgentId, ct);

        if (worker is null)
            return Error.NotFound(description: "Worker not found.");

        bool validPassword = _workerPasswordHasher.VerifyPassword(worker.PasswordHash, request.Password);

        if (!validPassword)
            return Error.Unauthorized(description: "Invalid Password.");

        if (worker.HardwareId != request.HardwareId)
            return Error.Validation(description: "Invalid HardwareId.");

        int expirationInSeconds = (int)TimeSpan.FromHours(6).TotalSeconds;
        string token = _workerTokenProvider.GenerateJwtToken(request.AgentId, expirationInSeconds);
        TokenResult result = new()
        {
            AccessToken = token,
            ExpiresIn = expirationInSeconds,
        };
        return result;
    }

    public async Task<ErrorOr<Updated>> UpdateCommandStatusAsync(
        Guid agentId,
        Guid commandId,
        CommandStatusRequest request,
        CancellationToken ct
        )
    {
        CommandAssignment? commandAssignment = await _workerRespository
            .GetAssignmentAsync(agentId, commandId, ct);

        if (commandAssignment == null)
            return Error.NotFound(description: "CommandAssignment not found.");

        AssignmentStatus newStatus = CommandStatusHelper.MapStatus(request.CommandStatus);
        AssignmentStatus current = commandAssignment.Status;

        if (!CommandStatusHelper.CanTransition(current, newStatus))
        {
            return Error.Conflict(description: CommandStatusHelper.TransitionErrorDescription(current, newStatus));
        }

        commandAssignment.Status = newStatus;

        if (newStatus == AssignmentStatus.InProgress)
        {
            commandAssignment.StartedAt = DateTimeOffset.UtcNow;
        }

        if (newStatus is AssignmentStatus.Done
            or AssignmentStatus.Failed
            or AssignmentStatus.Cancelled)
        {
            commandAssignment.FinishedAt = DateTimeOffset.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return Result.Updated;
    }
}
