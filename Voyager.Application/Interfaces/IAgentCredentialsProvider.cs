using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Interfaces;

public interface IAgentCredentialsProvider
{
    Task<VoyagerAgentCredentials> GetAgentCredentialsAsync(CancellationToken ct);
}
