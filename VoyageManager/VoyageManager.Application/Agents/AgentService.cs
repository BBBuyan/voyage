using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using VoyageManager.Application.Abstractions;
using VoyageManager.Conventions.Agents;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Agents;

internal class AgentService : IAgentService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IAgentRespository _voyagerAgentRespository;
    private readonly IVoyagePasswordHasher _voyagePasswordHasher;
    private readonly IVoyageTokenProvider _voyageTokenProvider;
    private readonly IMapper _mapper;

    public AgentService(
        ITenantRepository tenantRepository,
        IAgentRespository voyagerHostRespository,
        IVoyagePasswordHasher voyagePasswordHasher,
        IVoyageTokenProvider voyageTokenProvider,
        IMapper mapper
        )
    {
        _tenantRepository = tenantRepository;
        _voyagerAgentRespository = voyagerHostRespository;
        _voyagePasswordHasher = voyagePasswordHasher;
        _voyageTokenProvider = voyageTokenProvider;
        _mapper = mapper;
    }

    public async Task<ErrorOr<List<CommandAssignment>>> CheckIn(Guid agentId, CancellationToken ct)
    {
        VoyagerAgent? agent = await _voyagerAgentRespository
            .GetVoyagerAgentById(agentId, ct);

        if (agent == null)
        {
            return Error.NotFound(description: "Agent not found.");
        }

        if (!agent.IsEnabled)
        {
            return Error.Forbidden(description: "Agent is disabled.");
        }

        List<VoyagerCommand> commands = await _voyagerAgentRespository
            .GetVoyagerCommandsByAgentId(agentId, ct);

        return _mapper.Map<List<CommandAssignment>>(commands);
    }

    public async Task<ErrorOr<Guid>> Enroll(EnrollRequest request, CancellationToken ct)
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
            .RegisterVoyagerAgent(request.Name, hashedPassword, request.TenantId, ct);

        return agentId;
    }

    public async Task<ErrorOr<string>> GetToken(TokenRequest request, CancellationToken ct)
    {
        VoyagerAgent? agent = await _voyagerAgentRespository
            .GetVoyagerAgentById(request.AgentId, ct);

        if (agent is null)
        {
            return Error.NotFound(description: "Agent not found.");
        }

        bool passwordIsValid = _voyagePasswordHasher.VerifyPassword(agent.PasswordHash, request.Password);
        if (!passwordIsValid)
        {
            return Error.Unauthorized(description: "Invalid Password.");
        }

        string token = _voyageTokenProvider.GenerateJwtToken(request.AgentId);
        return token;
    }
}
