using System;
using System.Threading;
using System.Threading.Tasks;
using Voyager.Application.Abstractions;
using Voyager.Application.Interfaces;
using Voyager.Domain.Models;

namespace Voyager.Application.Services;

public class AgentCredentialsProvider : IAgentCredentialsProvider
{
    private readonly IAgentCredentialsStore _agentCredentialsStore;
    private readonly IEnrollmentCredentialsStore _enrollmentCredentialsStore;
    private readonly IVoyageManagerClient _voyageManagerClient;
    private VoyagerAgentCredentials? _agentCredentialsCached;

    public AgentCredentialsProvider(
        IAgentCredentialsStore agentCredentialsStore,
        IEnrollmentCredentialsStore enrollmentCredentialsStore,
        IVoyageManagerClient voyageManagerClient
    )
    {
        _agentCredentialsStore = agentCredentialsStore;
        _enrollmentCredentialsStore = enrollmentCredentialsStore;
        _voyageManagerClient = voyageManagerClient;
    }

    public async Task<VoyagerAgentCredentials> GetAgentCredentialsAsync(CancellationToken ct)
    {
        if (_agentCredentialsCached is not null)
            return _agentCredentialsCached;

        VoyagerAgentCredentials? creds = await _agentCredentialsStore.ReadAgentCredentialsAsync(ct);

        if (creds is null)
            creds = await EnrollAsync(ct);

        _agentCredentialsCached = creds;
        return creds;
    }

    public async Task<VoyagerAgentCredentials> EnrollAsync(CancellationToken ct)
    {
        EnrollmentCredentials? enrollmentCreds =
            await _enrollmentCredentialsStore.ReadEnrollmentCredentialsAsync(ct);
        if (enrollmentCreds is null)
            throw new InvalidOperationException("Could not read enrollment credentials.");

        VoyagerAgentCredentials agentCreds = await _voyageManagerClient.SendEnrollRequestAsync(
            enrollmentCreds,
            ct
        );

        await _agentCredentialsStore.SaveAgentCredentialsAsync(agentCreds, ct);
        //_enrollmentCredentialsStore.DeleteEnrollmentToken();

        _agentCredentialsCached = agentCreds;
        return agentCreds;
    }
}
