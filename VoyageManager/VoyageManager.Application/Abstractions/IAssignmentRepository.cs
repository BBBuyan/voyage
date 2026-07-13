using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface IAssignmentRepository
{
    Task<bool> CanCreateAssignmentAsync(CancellationToken ct);

    Task<IEnumerable<Worker>> GetWorkersByGroupIdAsync(Guid groupId, CancellationToken ct);

    Task<bool> WorkerExistsAsync(Guid id, CancellationToken ct);

    Task<bool> GroupExistsAsync(Guid id, CancellationToken ct);

    Task<CommandAssignment?> GetAssignmentAsync(Guid assignmentId, CancellationToken ct);

    Task<List<CommandAssignment>> GetAssignmentByWorkerIdAsync(Guid workerId, CancellationToken ct);

    Task<List<CommandAssignment>> GetAssignmentByGroupIdAsync(Guid groupId, CancellationToken ct);

    Task CreateAssignmentsAsync(IEnumerable<CommandAssignment> assignments, CancellationToken ct);
}
