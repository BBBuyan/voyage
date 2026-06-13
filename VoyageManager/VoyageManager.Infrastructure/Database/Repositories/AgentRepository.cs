using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoyageManager.Application.Abstractions;
using VoyageManager.Domain.Enums;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.Repositories;

public class AgentRepository : IAgentRespository
{
    private readonly VoyageManagerDbContext _dbContext;

    public AgentRepository(VoyageManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VoyagerCommandAssignment?> GetCommandAssignmentByAgentId(Guid agentId, CancellationToken ct)
    {
        return await _dbContext.VoyagerCommandAssignments.FirstOrDefaultAsync(x => x.Id == agentId, ct);
    }

    public async Task<List<VoyagerCommand>> GetPendingCommandsByAgentId(Guid agentId, CancellationToken ct)
    {
        return await _dbContext.VoyagerCommandAssignments
            .Where(x => x.VoyagerAgentId == agentId && x.Status == VoyagerCommandStatus.Pending)
            .Include(x => x.VoyagerCommand)
            .Select(x => x.VoyagerCommand)
            .ToListAsync(ct);
    }

    public async Task<VoyagerAgent?> GetVoyagerAgentById(Guid id, CancellationToken ct)
    {
        return await _dbContext
            .VoyagerAgents
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Guid> RegisterVoyagerAgent(string name, string passwordHash, Guid tenantId, CancellationToken ct)
    {
        VoyagerAgent newVoyagerHost = new()
        {
            Name = name,
            TenantId = tenantId,
            PasswordHash = passwordHash
        };
        _dbContext.VoyagerAgents.Add(newVoyagerHost);
        await _dbContext.SaveChangesAsync(ct);
        return newVoyagerHost.Id;
    }
}
