using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Application.Abstractions;

namespace VoyageManager.Infrastructure.Database.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly VoyageManagerDbContext _dbContext;

    public UnitOfWork(VoyageManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _dbContext.SaveChangesAsync(ct);
    }
}
