using System;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Api.Controllers.Base;
using VoyageManager.Application.Agents;
using VoyageManager.Conventions.Agents;

namespace VoyageManager.Api.Controllers;

/// <summary>
/// These endpoints are only accessed by VoyagerAgents running on customer devices. 
/// Endpoints are designated only for machine to machine communication.
/// </summary>
[Route("api/v1/workers")]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[Authorize(Policy = "WorkerOnly")]
public class WorkersController : BaseController
{
    private readonly IWorkerService _workerService;

    public WorkersController(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    /// <summary>
    /// Enroll to manager.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("enroll")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> Enroll([FromBody] EnrollRequest enrollRequest, CancellationToken ct)
    {
        ErrorOr<Guid> result = await _workerService.EnrollAsync(enrollRequest, ct);

        return result.MatchFirst<ActionResult>(
            value => Ok(value),
            err => ToProblem(err));
    }

    /// <summary>
    /// Check whether assignments exists or not.
    /// </summary>
    [HttpPost("check-in")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CheckInResponse>> CheckIn(CancellationToken ct)
    {
        string? workerIdString = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(workerIdString, out Guid workerId))
            return BadRequest("Invalid sub in token.");

        ErrorOr<CheckInResponse> result = await _workerService.CheckInAsync(workerId, ct);

        return result.MatchFirst<ActionResult>(
            value => Ok(value),
            err => ToProblem(err));
    }

    /// <summary>
    /// Get token.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TokenResult>> Token([FromBody] TokenRequest tokenRequest, CancellationToken ct)
    {
        ErrorOr<TokenResult> result = await _workerService.GetTokenAsync(tokenRequest, ct);

        return result.MatchFirst<ActionResult>(
            val => Ok(val),
            err => ToProblem(err));
    }

    /// <summary>
    /// Report the command results.
    /// </summary>
    /// <returns></returns>
    [HttpPut("assignments/{id}/status")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCommandStatus(Guid id, [FromBody] CommandStatusRequest request, CancellationToken ct)
    {
        string? agentIdString = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(agentIdString, out Guid agentId))
        {
            return BadRequest("Invalid AgentId");
        }

        ErrorOr<Updated> result = await _workerService
            .UpdateCommandStatusAsync(agentId, id, request, ct);

        return result.MatchFirst<ActionResult>(
            val => Ok(),
            err => ToProblem(err));
    }
}
