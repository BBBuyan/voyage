using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Abstractions;

public interface IAgentCredentialsStore
{
    Task<VoyagerAgentCredentials?> ReadAgentCredentialsAsync(CancellationToken ct);

    Task<bool> SaveAgentCredentialsAsync(VoyagerAgentCredentials credentials, CancellationToken ct);
}
