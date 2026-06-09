using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Application.Results;

namespace VoyageManager.Application.AgentManagement;

public interface IAgentManagementService
{
    Task<List<VoyagerAgentDTO>> GetAgents(CancellationToken ct);

    Task<int> EnableAgentById(Guid agentId, CancellationToken ct);

    Task<int> EnableAgentsByGroupId(Guid groupId, CancellationToken ct);
}
