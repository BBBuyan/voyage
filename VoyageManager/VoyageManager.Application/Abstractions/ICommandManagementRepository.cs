using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface ICommandManagementRepository
{
    Task<int> CreateCommandAsync(VoyagerCommand command, CancellationToken ct);
    Task<VoyagerCommand?> GetVoyagerCommandByIdAsync(Guid id, CancellationToken ct);
    Task<List<VoyagerCommand>> GetVoyagerCommandsAsync(CancellationToken ct);
    Task<List<VoyagerCommandAssignment>> GetVoyagerCommandAssignmentsByCommandIdAsync(Guid commandId, CancellationToken ct);
}
