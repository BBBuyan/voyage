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

    Task<ErrorOr<CommandStatusResponse>> UpdateCommandStatusAsync(Guid agentId, Guid commandId, CommandStatusRequest request, CancellationToken ct);
}
