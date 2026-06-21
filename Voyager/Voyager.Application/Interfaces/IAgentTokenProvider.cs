using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Interfaces;

public interface IAgentTokenProvider
{
    Task<AgentToken> GetTokenAsync(VoyagerAgentCredentials creds, CancellationToken ct);
}
