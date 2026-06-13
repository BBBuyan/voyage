using System.Threading;
using System.Threading.Tasks;
using Voyager.Application.Abstractions;
using Voyager.Application.Interfaces;
using Voyager.Domain.Models;

namespace Voyager.Application.Services;

public class AgentTokenProvider : IAgentTokenProvider
{
    private AgentToken? _agentTokenCached;
    private readonly IVoyageManagerClient _voyageManagerClient;

    public AgentTokenProvider(IVoyageManagerClient voyageManagerClient)
    {
        _voyageManagerClient = voyageManagerClient;
    }

    public async Task<AgentToken> GetTokenAsync(VoyagerAgentCredentials creds, CancellationToken ct)
    {
        if (_agentTokenCached is not null && !_agentTokenCached.IsExpired)
        {
            return _agentTokenCached;
        }

        AgentToken token = await _voyageManagerClient.FetchTokenAsync(creds, ct);

        _agentTokenCached = token;
        return token;
    }
}
