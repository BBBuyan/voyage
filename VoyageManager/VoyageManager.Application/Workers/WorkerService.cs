using System;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using VoyageManager.Application.Abstractions;
using VoyageManager.Application.Helpers;
using VoyageManager.Application.Mappings;
using VoyageManager.Conventions.Agents;
using VoyageManager.Domain.Engines;
using VoyageManager.Domain.Enums;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Workers;

public class WorkerService : IWorkerService
{
    private readonly IWorkerRespository _workerRespository;
    private readonly IWorkerPasswordHasher _workerPasswordHasher;
    private readonly IWorkerTokenProvider _workerTokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public WorkerService(
        IWorkerRespository workerRepository,
        IWorkerPasswordHasher passwordHasher,
        IWorkerTokenProvider tokenProvider,
        IUnitOfWork unitOfWork
    )
    {
        _workerRespository = workerRepository;
        _workerPasswordHasher = passwordHasher;
        _workerTokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<CheckInResponse>> CheckInAsync(
        Guid workerId,
        CheckInRequest request,
        CancellationToken ct
    )
    {
        Worker? worker = await _workerRespository.GetWorkerAsync(workerId, ct);
        if (worker is null)
            return Error.NotFound(description: "Worker not found.");

        if (!worker.IsEnabled)
        {
            return CheckInHelper.ShutdownResponse();
        }

        if (request.CurrentAssignmentId is not null)
        {
            CommandAssignment? assignment = await _workerRespository.GetAssignmentAsync(
                workerId,
                request.CurrentAssignmentId.Value,
                ct
            );
            if (assignment is null)
            {
                return Error.NotFound(description: "Assignment not found.");
            }
            if (assignment.State == AssignmentState.CancelRequested)
            {
                return CheckInHelper.CancelResponse();
            }
        }
        else
        {
            CommandAssignment? nextAssignment =
                await _workerRespository.GetNextPendingAssignmentAsync(workerId, ct);
            if (nextAssignment is not null)
            {
                return CheckInHelper.AssignmentResponse(nextAssignment);
            }
        }

        return CheckInHelper.ContinueResponse();
    }

    public async Task<ErrorOr<Guid>> EnrollAsync(EnrollRequest request, CancellationToken ct)
    {
        // Stage 1, validation
        Tenant? tenant = await _workerRespository.GetTenantById(request.TenantId, ct);

        if (tenant is null)
            return Error.NotFound(description: "Tenant not found.");

        if (tenant.EnrollmentSecret != request.EnrollmentSecret)
            return Error.Unauthorized(description: "Invalid EnrollmentToken.");

        if (tenant.AvailableLicenses == 0)
            return Error.Forbidden(description: "No licenses are available.");

        // Stage 2, creation
        string hashedPassword = _workerPasswordHasher.HashPassword(request.Password);
        Guid agentId = await _workerRespository.RegisterWorkerAsync(
            request.Name,
            request.HardwareId,
            hashedPassword,
            tenant.Id,
            ct
        );

        return agentId;
    }

    public async Task<ErrorOr<TokenResult>> GetTokenAsync(
        TokenRequest request,
        CancellationToken ct
    )
    {
        Worker? worker = await _workerRespository.GetWorkerAsync(request.AgentId, ct);

        if (worker is null)
            return Error.NotFound(description: "Worker not found.");

        bool validPassword = _workerPasswordHasher.VerifyPassword(
            worker.PasswordHash,
            request.Password
        );

        if (!validPassword)
            return Error.Unauthorized(description: "Invalid Password.");

        if (worker.HardwareId != request.HardwareId)
            return Error.Validation(description: "Invalid HardwareId.");

        int expirationInSeconds = (int)TimeSpan.FromHours(6).TotalSeconds;
        string token = _workerTokenProvider.GenerateJwtToken(request.AgentId, expirationInSeconds);
        TokenResult result = new() { AccessToken = token, ExpiresIn = expirationInSeconds };
        return result;
    }

    public async Task<ErrorOr<Updated>> UpdateCommandStatusAsync(
        Guid agentId,
        Guid commandId,
        UpdateAssignmentStateRequest request,
        CancellationToken ct
    )
    {
        CommandAssignment? commandAssignment = await _workerRespository.GetAssignmentAsync(
            agentId,
            commandId,
            ct
        );

        if (commandAssignment == null)
            return Error.NotFound(description: "CommandAssignment not found.");

        AssignmentState newStatus = request.AssignmentState.ToDomain();
        AssignmentState current = commandAssignment.State;

        if (!CommandAssignmentEngine.CanTransition(current, newStatus))
        {
            return Error.Conflict(
                description: AssignmentStatusHelper.TransitionErrorDescription(current, newStatus)
            );
        }

        commandAssignment.State = newStatus;

        if (newStatus == AssignmentState.InProgress)
        {
            commandAssignment.StartedAt = DateTimeOffset.UtcNow;
        }

        if (newStatus is AssignmentState.Done or AssignmentState.Failed)
        {
            commandAssignment.FinishedAt = DateTimeOffset.UtcNow;
        }

        if (newStatus is AssignmentState.Cancelled)
        {
            commandAssignment.CancelledAt = DateTimeOffset.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return Result.Updated;
    }
}
