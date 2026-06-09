using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Abstractions;

public interface IAgentCredentialsStore
{
    Task<VoyagerAgentCredentials?> ReadAgentCredentials(CancellationToken ct);

    Task<bool> SaveAgentCredentials(VoyagerAgentCredentials credentials, CancellationToken ct);
}
