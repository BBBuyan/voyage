using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using VoyageManager.Conventions.Agents;

namespace VoyageManager.Application.Agents;

public interface IAgentService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="agentId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<ErrorOr<List<CheckInResponse>>> CheckIn(Guid agentId, CancellationToken ct);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<ErrorOr<Guid>> Enroll(EnrollRequest request, CancellationToken ct);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<ErrorOr<TokenResult>> GetToken(TokenRequest request, CancellationToken ct);

    Task<ErrorOr<CommandStatusResponse>> HandleCommandReports(Guid agentId, CommandStatusRequest request, CancellationToken ct);
}
