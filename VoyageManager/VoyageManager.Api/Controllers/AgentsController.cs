using System;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Application.Agents;
using VoyageManager.Conventions.Agents;

namespace VoyageManager.Api.Controllers;

/// <summary>
/// These endpoints are only accessed by VoyagerAgents running on customer devices. 
/// Endpoints are designated only for machine to machine communication.
/// </summary>
[ApiController]
[Route("api/agents")]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[Authorize(Policy = "AgentOnly")]
public class AgentsController : ControllerBase
{
    private readonly IAgentService _agentService;

    public AgentsController(IAgentService agentService)
    {
        _agentService = agentService;
    }

    /// <summary>
    /// Enroll to manager.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("enroll")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> Enroll([FromBody] EnrollRequest enrollRequest, CancellationToken ct)
    {
        ErrorOr<Guid> result = await _agentService.EnrollAsync(enrollRequest, ct);
        Console.WriteLine(result);

        return result.MatchFirst<ActionResult>(
            onValue => Ok(onValue),
            onError => onError.Type switch
            {
                ErrorType.NotFound => NotFound(onError.Description),
                ErrorType.Unauthorized => Unauthorized(onError.Description),
                _ => Problem(),
            });
    }

    /// <summary>
    /// Check whether commands exists or not.
    /// </summary>
    [HttpGet("check-in")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CheckInResponse>> CheckIn(CancellationToken ct)
    {
        string? agentIdString = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(agentIdString, out Guid agentId))
        {
            return BadRequest("Invalid AgentId");
        }

        ErrorOr<CheckInResponse> result = await _agentService.CheckInAsync(agentId, ct);

        return result.MatchFirst<ActionResult>(
            value => Ok(value),
            error => error.Type switch
            {
                ErrorType.NotFound => NotFound(error.Description),
                ErrorType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, error.Description),
                _ => Problem()
            });
    }

    /// <summary>
    /// Get token.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TokenResult>> Token([FromBody] TokenRequest tokenRequest, CancellationToken ct)
    {
        ErrorOr<TokenResult> result = await _agentService.GetTokenAsync(tokenRequest, ct);

        return result.MatchFirst<ActionResult>(
            onValue => Ok(onValue),
            onError => onError.Type switch
            {
                ErrorType.NotFound => NotFound(onError.Description),
                ErrorType.Unauthorized => Unauthorized(onError.Description),
                _ => Problem(),
            });
    }

    /// <summary>
    /// Report the command results.
    /// </summary>
    /// <returns></returns>
    [HttpPost("commands/{id}/status")]
    public async Task<ActionResult<CommandStatusResponse>> UpdateCommandStatus(Guid id, [FromBody] CommandStatusRequest request, CancellationToken ct)
    {
        string? agentIdString = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(agentIdString, out Guid agentId))
        {
            return BadRequest("Invalid AgentId");
        }

        ErrorOr<CommandStatusResponse> result = await _agentService
            .UpdateCommandStatusAsync(agentId, id, request, ct);

        return result.MatchFirst<ActionResult>(
            value => Ok(value),
            err => err.Type switch
            {
                ErrorType.NotFound => NotFound(err.Description),
                _ => Problem()
            });
    }
}
