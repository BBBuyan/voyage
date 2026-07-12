using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface ICommandManagementRepository
{
    Task<List<CommandAssignment>> GetVoyagerCommandAssignmentsByCommandIdAsync(Guid commandId, CancellationToken ct);
}
