using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Abstractions;

public interface IVoyageManagerClient
{
    Task<VoyagerAgentCredentials> SendEnrollRequestAsync(
        EnrollmentCredentials enrollmentCreds,
        CancellationToken ct
    );

    Task<AgentCommand?> CheckInAsync(string token, CancellationToken ct);

    Task<AgentToken> FetchTokenAsync(VoyagerAgentCredentials creds, CancellationToken ct);

    Task SendCommandStatus(string token, AgentCommand agentCommand, CancellationToken ct);
}
