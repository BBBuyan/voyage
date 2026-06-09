using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoyageManager.Application.Abstractions;
using VoyageManager.Domain.Models;

namespace VoyageManager.Infrastructure.Database.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly VoyageManagerDbContext _dbContext;

    public TenantRepository(VoyageManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Tenant?> GetTenantById(Guid tenantId, CancellationToken ct)
    {
        Tenant? tenant = await _dbContext.Tenants
            .Where(x => x.Id == tenantId)
            .FirstOrDefaultAsync(ct);
        return tenant;
    }
}
