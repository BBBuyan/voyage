using System;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Api.Controllers.Base;
using VoyageManager.Application.Workers;
using VoyageManager.Conventions.Agents;

namespace VoyageManager.Api.Controllers;

/// <summary>
/// These endpoints are only accessed by Workers running on customer devices.
/// Endpoints are designated only for machine to machine communication.
/// </summary>
[Route("api/v1/workers")]
[Consumes("application/json")]
[Produces("application/json")]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> Enroll(
        [FromBody] EnrollRequest enrollRequest,
        CancellationToken ct
    )
    {
        ErrorOr<Guid> result = await _workerService.EnrollAsync(enrollRequest, ct);

        return result.MatchFirst<ActionResult>(value => Ok(value), err => ToProblem(err));
    }

    /// <summary>
    /// Check whether assignments exists or not.
    /// </summary>
    [HttpPost("check-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CheckInResponse>> CheckIn(
        CheckInRequest request,
        CancellationToken ct
    )
    {
        string? workerIdString = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(workerIdString, out Guid workerId))
            return Unauthorized("Invalid sub in token.");

        ErrorOr<CheckInResponse> result = await _workerService.CheckInAsync(workerId, request, ct);

        return result.MatchFirst<ActionResult>(value => Ok(value), err => ToProblem(err));
    }

    /// <summary>
    /// Get token.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TokenResponse>> Token(
        [FromBody] TokenRequest tokenRequest,
        CancellationToken ct
    )
    {
        ErrorOr<TokenResponse> result = await _workerService.GetTokenAsync(tokenRequest, ct);

        return result.MatchFirst<ActionResult>(val => Ok(val), err => ToProblem(err));
    }

    /// <summary>
    /// Update the assignment state.
    /// </summary>
    /// <returns></returns>
    [HttpPut("assignments/{id}/state")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateCommandStatus(
        Guid id,
        [FromBody] UpdateAssignmentStateRequest request,
        CancellationToken ct
    )
    {
        string? agentIdString = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(agentIdString, out Guid agentId))
            return Unauthorized("Invalid sub in token.");

        ErrorOr<Updated> result = await _workerService.UpdateCommandStatusAsync(
            agentId,
            id,
            request,
            ct
        );

        return result.MatchFirst<ActionResult>(val => NoContent(), err => ToProblem(err));
    }
}
