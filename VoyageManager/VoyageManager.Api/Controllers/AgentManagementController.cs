using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Api.DTOs;
using VoyageManager.Application.DTOs;
using VoyageManager.Application.Interfaces;

namespace VoyageManager.Api.Controllers;

[ApiController]
[Route("api/management/agents")]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AgentManagementController : ControllerBase
{
    private readonly IAgentManagementService _agentManagementService;

    public AgentManagementController(IAgentManagementService service)
    {
        _agentManagementService = service;
    }

    [AllowAnonymous]
    [HttpGet()]
    public async Task<ActionResult<List<VoyagerAgentDTO>>> GetVoyagers(CancellationToken ct)
    {
        List<VoyagerAgentDTO> voyagerAgents = await _agentManagementService.GetAgents(ct);
        return Ok(voyagerAgents);
    }

    [AllowAnonymous]
    [HttpPost("enable")]
    public async Task<ActionResult<int>> EnableById(EnableAgentsRequest request, CancellationToken ct)
    {
        int affected = 0;
        if (request.AgentId is not null)
        {
            affected = await _agentManagementService
                .EnableAgentById(request.AgentId.Value, ct);
        }
        else if (request.GroupId is not null)
        {
            affected = await _agentManagementService
                .EnableAgentsByGroupId(request.GroupId.Value, ct);
        }

        return Ok(affected);
    }
}
