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
public class AssignmentsController : BaseController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentsController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CommandAssignmentDTO>>> GetAssignments(
        Guid? workerId,
        Guid? groupId,
        CancellationToken ct
    )
    {
        ErrorOr<List<CommandAssignmentDTO>> result = await _assignmentService.GetAssignmentsAsync(
            workerId,
            groupId,
            ct
        );

        return result.MatchFirst<ActionResult>(value => Ok(value), err => ToProblem(err));
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateAssignment(
        CreateAssignmentRequest request,
        CancellationToken ct
    )
    {
        ErrorOr<Success> result = await _assignmentService.CreateAssignmentAsync(
            request,
            "tc1-user1",
            ct
        );
        return result.MatchFirst<ActionResult>(value => NoContent(), err => ToProblem(err));
    }

    [AllowAnonymous]
    [HttpPut("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CancelAssignment(Guid id, CancellationToken ct)
    {
        ErrorOr<Success> result = await _assignmentService.CancelAssignmentAsync(id, ct);
        return result.MatchFirst<ActionResult>(value => NoContent(), err => ToProblem(err));
    }
}
