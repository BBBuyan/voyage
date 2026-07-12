using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Api.Controllers.Base;
using VoyageManager.Application.DTOs;
using VoyageManager.Application.Interfaces;

namespace VoyageManager.Api.Controllers;

[Route("api/v1/assignments")]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AssignmentController : BaseController
{
    private readonly ICommandService _commandService;

    public AssignmentController(ICommandService commandManagementService)
    {
        _commandService = commandManagementService;
    }

    [AllowAnonymous]
    [HttpGet("{id}/assignments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<VoyagerCommandAssignmentDTO>>> GetCommandAssignments(Guid id, CancellationToken ct)
    {
        ErrorOr<List<VoyagerCommandAssignmentDTO>> result = await _commandService.GetCommandAssignmentsByCommandId(id, ct);

        return result.MatchFirst<ActionResult>(
            value => Ok(value),
            err => err.Type switch
            {
                ErrorType.NotFound => NotFound(err.Description),
                _ => Problem()
            });
    }
}
