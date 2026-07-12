using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Api.Controllers.Base;
using VoyageManager.Application.DTOs;
using VoyageManager.Application.Interfaces;

namespace VoyageManager.Api.Controllers;

[Route("api/v1/management/workers")]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class WorkerManagementController : BaseController
{
    private readonly IWorkerManagementService _agentManagementService;

    public WorkerManagementController(IWorkerManagementService service)
    {
        _agentManagementService = service;
    }

    /// <summary>
    /// Get agents.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<VoyagerWorkerDTO>>> GetVoyagers(CancellationToken ct)
    {
        List<VoyagerWorkerDTO> voyagerAgents = await _agentManagementService.GetAgentsAsync(ct);
        return Ok(voyagerAgents);
    }

    /// <summary>
    /// Update agent status.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("{id:guid}/status")]
    public async Task<ActionResult<int>> UpdateStatus(Guid id, CancellationToken ct)
    {
        int affected = await _agentManagementService.EnableAgentAsync(id, ct);

        return Ok(affected);
    }

    /// <summary>
    /// Get all command assignments for the specified agent.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("{id:guid}/assignments")]
    public async Task<ActionResult<VoyagerCommandAssignmentDTO>> GetCommands(Guid id, CancellationToken ct)
    {
        return Ok();
    }

    /// <summary>
    /// Create command assignment.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("{id:guid}/assignments")]
    public async Task<ActionResult> CreateAssignment(Guid id, CancellationToken ct)
    {
        return Ok();
    }
}
