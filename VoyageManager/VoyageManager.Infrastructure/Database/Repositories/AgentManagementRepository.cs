using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoyageManager.Application.Abstractions;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.Repositories;

public class AgentManagementRepository : IAgentManagementRepository
{
    private readonly VoyageManagerDbContext _dbContext;

    public AgentManagementRepository(VoyageManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Worker>> GetVoyagerAgents(CancellationToken ct)
    {
        return await _dbContext.Workers.ToListAsync(ct);
    }

    public Task<bool> AgentExistsAsync(Guid agentId, CancellationToken ct)
    {
        return _dbContext.Workers.AnyAsync(x => x.Id == agentId, ct);
    }

    public Task<List<Guid>> GetVoyagerAgentIdsByTenantId(Guid tenantId, CancellationToken ct)
    {
        return _dbContext.Workers
            .Where(x => x.TenantId == tenantId)
            .Select(x => x.Id)
            .ToListAsync(ct);
    }

    public Task<List<Guid>> GetVoyagerAgentIdsByGroupId(Guid groupId, CancellationToken ct)
    {
        return _dbContext.GroupAssignments
            .Where(x => x.GroupId == groupId)
            .Select(x => x.WorkerId)
            .ToListAsync(ct);
    }

    public async Task<int> EnableAgentById(Guid agentId, CancellationToken ct)
    {
        return await _dbContext.Workers
            .Where(x => x.Id == agentId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsEnabled, true), ct);
    }

    public async Task<int> EnableAgentsByGroupId(Guid groupId, CancellationToken ct)
    {
        List<Guid> agentIds = await _dbContext.GroupAssignments
            .Where(x => x.GroupId == groupId)
            .Select(x => x.WorkerId)
            .ToListAsync(ct);

        return await _dbContext.Workers
            .Where(x => agentIds.Contains(x.Id))
            .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsEnabled, true), ct);
    }


}
