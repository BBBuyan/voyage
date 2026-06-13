using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface IAgentRespository
{
    Task<VoyagerAgent?> GetVoyagerAgentById(Guid id, CancellationToken ct);

    Task<List<VoyagerCommand>> GetPendingCommandsByAgentId(Guid agentId, CancellationToken ct);

    Task<Guid> RegisterVoyagerAgent(string name, string passwordHash, Guid tenantId, CancellationToken ct);

    Task<VoyagerCommandAssignment?> GetCommandAssignmentByAgentId(Guid agentId, CancellationToken ct);
}
