using System.Threading;
using System.Threading.Tasks;

namespace VoyageManager.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}
