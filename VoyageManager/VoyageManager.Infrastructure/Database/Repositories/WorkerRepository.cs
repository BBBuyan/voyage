using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoyageManager.Application.Abstractions;
using VoyageManager.Domain.Enums;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.Repositories;

public class WorkerRepository : IWorkerRespository
{
    private readonly ApplicationDbContext _dbContext;

    public WorkerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CommandAssignment?> GetAssignmentAsync(
        Guid agentId,
        Guid assignmentId,
        CancellationToken ct
    )
    {
        return await _dbContext.CommandAssignments.FirstOrDefaultAsync(
            x => x.WorkerId == agentId && x.Id == assignmentId,
            ct
        );
    }

    public async Task<CommandAssignment?> GetNextPendingAssignmentAsync(
        Guid agentId,
        CancellationToken ct
    )
    {
        return await _dbContext
            .CommandAssignments.Where(x =>
                x.WorkerId == agentId && x.State == AssignmentState.Pending
            )
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<Worker?> GetWorkerAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Workers.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Guid> RegisterWorkerAsync(
        string name,
        string hardwareId,
        string passwordHash,
        Guid tenantId,
        CancellationToken ct
    )
    {
        Worker newWorker = new()
        {
            Name = name,
            PasswordHash = passwordHash,
            HardwareId = hardwareId,
            TenantId = tenantId,
        };
        _dbContext.Workers.Add(newWorker);
        await _dbContext.SaveChangesAsync(ct);
        return newWorker.Id;
    }

    public async Task<Tenant?> GetTenantById(Guid tenantId, CancellationToken ct)
    {
        Tenant? tenant = await _dbContext
            .Tenants.Where(x => x.Id == tenantId)
            .FirstOrDefaultAsync(ct);
        return tenant;
    }
}
