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
    private readonly IWorkerManagementService _workerManagementService;

    public WorkerManagementController(IWorkerManagementService service)
    {
        _workerManagementService = service;
    }

    /// <summary>
    /// Get agents.
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<WorkerDTO>>> GetWorkers(CancellationToken ct)
    {
        List<WorkerDTO> voyagerAgents = await _workerManagementService.GetAgentsAsync(ct);
        return Ok(voyagerAgents);
    }

    /// <summary>
    /// Update worker state: Enabled or Disabled
    /// </summary>
    [AllowAnonymous]
    [HttpPut("status")]
    public async Task<ActionResult<int>> UpdateStatus(
        Guid? workerId,
        Guid? groupId,
        CancellationToken ct
    )
    {
        if (workerId.HasValue)
        {
            int affected = await _workerManagementService.EnableAgentAsync(workerId.Value, ct);
            return Ok(affected);
        }

        return Ok();
    }
}
