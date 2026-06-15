using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using Microsoft.Extensions.Logging;
using VoyageManager.Application.Abstractions;
using VoyageManager.Conventions.Agents;
using VoyageManager.Conventions.Enums;
using VoyageManager.Domain.Enums;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Agents;

public class AgentService : IAgentService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IAgentRespository _voyagerAgentRespository;
    private readonly IVoyagePasswordHasher _voyagePasswordHasher;
    private readonly IVoyageTokenProvider _voyageTokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AgentService> _logger;

    public AgentService(
        ITenantRepository tenantRepository,
        IAgentRespository voyagerHostRespository,
        IVoyagePasswordHasher voyagePasswordHasher,
        IVoyageTokenProvider voyageTokenProvider,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AgentService> logger
        )
    {
        _tenantRepository = tenantRepository;
        _voyagerAgentRespository = voyagerHostRespository;
        _voyagePasswordHasher = voyagePasswordHasher;
        _voyageTokenProvider = voyageTokenProvider;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ErrorOr<Guid>> EnrollAsync(EnrollRequest request, CancellationToken ct)
    {
        // Stage 1, validation
        Tenant? tenant = await _tenantRepository
            .GetTenantById(request.TenantId, ct);

        if (tenant is null)
        {
            return Error.NotFound(description: "Tenant not found.");
        }

        if (tenant.EnrollmentSecret != request.EnrollmentSecret)
        {
            return Error.Unauthorized(description: "Invalid EnrollmentToken.");
        }

        if (tenant.AvailableLicenses == 0)
        {
            return Error.Forbidden(description: "No licenses are available.");
        }

        // Stage 2, creation
        string hashedPassword = _voyagePasswordHasher.HashPassword(request.Password);
        Guid agentId = await _voyagerAgentRespository
            .RegisterAgentAsync(request.Name, hashedPassword, request.TenantId, ct);

        return agentId;
    }

    public async Task<ErrorOr<TokenResult>> GetTokenAsync(TokenRequest request, CancellationToken ct)
    {
        VoyagerAgent? agent = await _voyagerAgentRespository
            .GetAgentAsync(request.AgentId, ct);

        if (agent is null)
        {
            return Error.NotFound(description: "Agent not found.");
        }

        bool passwordIsValid = _voyagePasswordHasher.VerifyPassword(agent.PasswordHash, request.Password);
        if (!passwordIsValid)
        {
            return Error.Unauthorized(description: "Invalid Password.");
        }

        int expirationInSeconds = (int)TimeSpan.FromHours(6).TotalSeconds;
        string token = _voyageTokenProvider.GenerateJwtToken(request.AgentId, expirationInSeconds);
        TokenResult result = new()
        {
            AccessToken = token,
            ExpiresIn = expirationInSeconds,
        };
        return result;
    }

    public async Task<ErrorOr<CheckInResponse>> CheckInAsync(Guid agentId, CancellationToken ct)
    {
        VoyagerAgent? agent = await _voyagerAgentRespository
            .GetAgentAsync(agentId, ct);

        if (agent == null)
            return Error.NotFound(description: "Agent not found.");

        if (!agent.IsEnabled)
            return Error.Forbidden(description: "Agent is disabled.");

        VoyagerCommand? commands = await _voyagerAgentRespository
            .GetPendingCommandAsync(agentId, ct);

        return _mapper.Map<CheckInResponse>(commands);
    }

    /// <summary>
    /// Handles the command status report that agent has sent.
    /// </summary>
    /// <remarks>
    /// If command assignment is not found for the specified agent, 
    /// returns <see cref="ConventionCommandResponseType.Stop"/> to stop the agent
    /// running a command that it is not supposed to.
    /// </remarks>
    public async Task<ErrorOr<CommandStatusResponse>> UpdateCommandStatusAsync(
        Guid agentId,
        Guid commandId,
        CommandStatusRequest request,
        CancellationToken ct
        )
    {
        VoyagerCommandAssignment? commandAssignment = await _voyagerAgentRespository
            .GetCommandAssignmentAsync(agentId, commandId, ct);

        if (commandAssignment == null)
        {
            return Error.NotFound(description: "CommandAssignment not found");
        }
        VoyagerCommandStatus localStatus = commandAssignment.Status;

        if (localStatus == VoyagerCommandStatus.Succeeded
            || localStatus == VoyagerCommandStatus.Cancelled)
        {
            return new CommandStatusResponse() { ResponseType = ConventionCommandResponseType.Stop };
        }

        commandAssignment.Status = (VoyagerCommandStatus)request.CommandStatus;

        if (request.CommandStatus == ConventionCommandStatus.InProgress)
        {
            commandAssignment.StartedAt = DateTimeOffset.UtcNow;
        }
        if (request.CommandStatus == ConventionCommandStatus.Succeeded)
        {
            commandAssignment.FinishedAt = DateTimeOffset.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync(ct);

        return new CommandStatusResponse() { ResponseType = ConventionCommandResponseType.Continue };
    }
}
