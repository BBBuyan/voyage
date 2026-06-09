using System;
using System.Collections.Generic;
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
        ErrorOr<Guid> result = await _agentService.Enroll(enrollRequest, ct);
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
    [HttpPost("check-in")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CommandAssignment>>> CheckIn(CancellationToken ct)
    {
        string? agentIdString = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(agentIdString, out Guid agentId))
        {
            return BadRequest("Invalid AgentId");
        }

        ErrorOr<List<CommandAssignment>> result = await _agentService.CheckIn(agentId, ct);

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
    public async Task<ActionResult<string>> Token([FromBody] TokenRequest tokenRequest, CancellationToken ct)
    {
        ErrorOr<string> result = await _agentService.GetToken(tokenRequest, ct);

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
    [HttpPost("results")]
    public async Task<ActionResult<ResultsResponse>> Result([FromBody] ResultsRequest resultsRequest, CancellationToken ct)
    {
        return Ok(new());
    }
}
