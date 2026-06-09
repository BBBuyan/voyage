using System;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Conventions.Agents;
using Voyager.Application.Abstractions;
using Voyager.Application.Interfaces;
using Voyager.Domain.Models;

namespace Voyager.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IVoyageManagerClient _voyageManagerClient;
    private readonly IAgentCredentialsStore _agentCredentialsStore;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IEnrollmentCredentialsStore _enrollmentCredentialsStore;

    public EnrollmentService(
        IVoyageManagerClient voyageManagerClient,
        IAgentCredentialsStore stateStore,
        IPasswordGenerator passwordGenerator,
        IEnrollmentCredentialsStore enrollmentCredentialsStore
        )
    {
        _voyageManagerClient = voyageManagerClient;
        _agentCredentialsStore = stateStore;
        _passwordGenerator = passwordGenerator;
        _enrollmentCredentialsStore = enrollmentCredentialsStore;
    }

    public async Task<VoyagerAgentCredentials> Enroll(CancellationToken ct)
    {
        EnrollmentCredentials? enrollmentCreds = await _enrollmentCredentialsStore.ReadEnrollmentCredentials(ct);
        if (enrollmentCreds is null)
            throw new Exception("Could not read Enrollment Credentials");

        string password = _passwordGenerator.GeneratePassword();
        string name = $"{Environment.MachineName}-{Environment.OSVersion.Platform}-{Environment.UserName}";

        EnrollRequest enrollRequest = new()
        {
            TenantId = enrollmentCreds.TenantId,
            EnrollmentSecret = enrollmentCreds.EnrollmentSecret,
            Password = password,
            Name = name
        };
        Guid agentId = await _voyageManagerClient.SendEnrollRequest(enrollRequest);

        VoyagerAgentCredentials agentCreds = new()
        {
            AgentId = agentId,
            Name = name,
            TenantId = enrollmentCreds.TenantId,
            Password = password,
        };

        await _agentCredentialsStore.SaveAgentCredentials(agentCreds, ct);
        //_enrollmentCredentialsStore.DeleteEnrollmentToken();
        return agentCreds;
    }
}
