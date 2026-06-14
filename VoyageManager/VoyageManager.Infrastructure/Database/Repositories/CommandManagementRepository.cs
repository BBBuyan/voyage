using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoyageManager.Application.Abstractions;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.Repositories;

public class CommandManagementRepository : ICommandManagementRepository
{
    private readonly VoyageManagerDbContext _dbContext;

    public CommandManagementRepository(VoyageManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateCommandAsync(VoyagerCommand command, CancellationToken ct)
    {
        _dbContext.VoyagerCommands.Add(command);
        return await _dbContext.SaveChangesAsync(ct);
    }

    public Task<List<VoyagerCommandAssignment>> GetVoyagerCommandAssignmentsByCommandIdAsync(Guid commandId, CancellationToken ct)
    {
        return _dbContext.VoyagerCommandAssignments.Where(x => x.VoyagerCommandId == commandId).ToListAsync(ct);
    }

    public Task<VoyagerCommand?> GetVoyagerCommandByIdAsync(Guid id, CancellationToken ct)
    {
        return _dbContext.VoyagerCommands.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task<List<VoyagerCommand>> GetVoyagerCommandsAsync(CancellationToken ct)
    {
        return _dbContext.VoyagerCommands.ToListAsync(ct);
    }
}
