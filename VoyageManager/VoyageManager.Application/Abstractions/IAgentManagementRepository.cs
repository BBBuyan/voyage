using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface IAgentManagementRepository
{
    Task<bool> AgentExistsAsync(Guid agentId, CancellationToken ct);

    Task<List<VoyagerAgent>> GetVoyagerAgents(CancellationToken ct);

    Task<List<Guid>> GetVoyagerAgentIdsByTenantId(Guid tenantId, CancellationToken ct);

    Task<List<Guid>> GetVoyagerAgentIdsByGroupId(Guid groupId, CancellationToken ct);

    Task<int> EnableAgentById(Guid agentId, CancellationToken ct);

    Task<int> EnableAgentsByGroupId(Guid groupId, CancellationToken ct);
}
