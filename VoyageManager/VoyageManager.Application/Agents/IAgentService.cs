using System;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using VoyageManager.Conventions.Agents;

namespace VoyageManager.Application.Agents;

public interface IAgentService
{
    Task<ErrorOr<CheckInResponse>> CheckInAsync(Guid agentId, CancellationToken ct);

    Task<ErrorOr<Guid>> EnrollAsync(EnrollRequest request, CancellationToken ct);

    Task<ErrorOr<TokenResult>> GetTokenAsync(TokenRequest request, CancellationToken ct);

    /// <summary>
    /// Handles the command status report that agent has sent.
    /// </summary>
    /// <remarks>
    /// Possible errors:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="ErrorType.NotFound"/>: if command not found.</description>
    /// </item>
    /// <item>
    /// <description><see cref="ErrorType.Forbidden"/>: if the transition is invalid.</description>
    /// </item>
    /// </list>
    /// </remarks>
    Task<ErrorOr<Updated>> UpdateCommandStatusAsync(Guid agentId, Guid commandId, CommandStatusRequest request, CancellationToken ct);
}
