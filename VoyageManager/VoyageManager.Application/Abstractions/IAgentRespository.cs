using System;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface IAgentRespository
{
    Task<VoyagerAgent?> GetAgentAsync(Guid id, CancellationToken ct);

    Task<VoyagerCommand?> GetPendingCommandAsync(Guid agentId, CancellationToken ct);

    Task<VoyagerCommandAssignment?> GetCommandAssignmentAsync(Guid agentId, Guid commandId, CancellationToken ct);

    Task<Guid> RegisterAgentAsync(
        string name,
        string hardwareId,
        string passwordHash,
        Guid tenantId,
        CancellationToken ct);
}
