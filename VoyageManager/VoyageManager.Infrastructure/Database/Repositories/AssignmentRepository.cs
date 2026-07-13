using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoyageManager.Application.Abstractions;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AssignmentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CanCreateAssignmentAsync(CancellationToken ct)
    {
        Tenant? tenant = await _dbContext.Tenants.FirstOrDefaultAsync(ct);

        if (tenant is null)
        {
            return false;
        }

        return tenant.CanCreateAssignments;
    }

    public Task<IEnumerable<Worker>> GetWorkersByGroupIdAsync(Guid groupId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAssignmentsAsync(
        IEnumerable<CommandAssignment> assignments,
        CancellationToken ct
    )
    {
        _dbContext.CommandAssignments.AddRange(assignments);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<List<CommandAssignment>> GetAssignmentByWorkerIdAsync(
        Guid workerId,
        CancellationToken ct
    )
    {
        return await _dbContext
            .CommandAssignments.Where(x => x.WorkerId == workerId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<CommandAssignment>> GetAssignmentByGroupIdAsync(
        Guid groupId,
        CancellationToken ct
    )
    {
        List<Guid> workerIds = await _dbContext
            .GroupAssignments.Where(x => x.GroupId == groupId)
            .Select(x => x.WorkerId)
            .ToListAsync(ct);

        return await _dbContext
            .CommandAssignments.Where(x => workerIds.Contains(x.WorkerId))
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public Task<bool> WorkerExistsAsync(Guid id, CancellationToken ct)
    {
        return _dbContext.Workers.AnyAsync(x => x.Id == id, ct);
    }

    public Task<bool> GroupExistsAsync(Guid id, CancellationToken ct)
    {
        return _dbContext.Groups.AnyAsync(x => x.Id == id, ct);
    }

    public Task<CommandAssignment?> GetAssignmentAsync(Guid assignmentId, CancellationToken ct)
    {
        return _dbContext.CommandAssignments.FirstOrDefaultAsync(x => x.Id == assignmentId, ct);
    }
}
