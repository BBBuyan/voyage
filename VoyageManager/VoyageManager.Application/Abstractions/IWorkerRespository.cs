using System;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface IWorkerRespository
{
    Task<Worker?> GetWorkerAsync(Guid id, CancellationToken ct);

    Task<CommandAssignment?> GetAssignmentAsync(Guid workerId, Guid assignmentId, CancellationToken ct);

    Task<CommandAssignment?> GetNextPendingAssignmentAsync(Guid workerId, CancellationToken ct);

    Task<Guid> RegisterWorkerAsync(
        string name,
        string hardwareId,
        string passwordHash,
        Guid tenantId,
        CancellationToken ct);

    Task<Tenant?> GetTenantById(Guid tenantId, CancellationToken ct);
}
