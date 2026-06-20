using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Api.DTOs;
using VoyageManager.Application.DTOs;
using VoyageManager.Application.Interfaces;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Api.Controllers;

[ApiController]
[Route("api/v1/commands")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CommandManagementController : ControllerBase
{
    private readonly ICommandManagementService _commandManagementService;

    public CommandManagementController(ICommandManagementService commandManagementService)
    {
        _commandManagementService = commandManagementService;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VoyagerCommandDTO>> GetCommandById(Guid id, CancellationToken ct)
    {
        ErrorOr<VoyagerCommandDTO> result = await _commandManagementService.GetCommandByIdAsync(id, ct);

        return result.MatchFirst<ActionResult>(
            value => Ok(value),
            err => err.Type switch
            {
                ErrorType.NotFound => NotFound(err.Description),
                _ => Problem()
            });
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<VoyagerCommandDTO>> GetCommands(CancellationToken ct)
    {
        ErrorOr<List<VoyagerCommandDTO>> result = await _commandManagementService.GetCommandsAsync(ct);

        return result.MatchFirst<ActionResult>(
            value => Ok(value),
            err => Problem()
            );
    }

    [AllowAnonymous]
    [HttpGet("{id}/assignments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<VoyagerCommandAssignmentDTO>>> GetCommandAssignments(Guid id, CancellationToken ct)
    {
        ErrorOr<List<VoyagerCommandAssignmentDTO>> result = await _commandManagementService.GetCommandAssignmentsByCommandId(id, ct);

        return result.MatchFirst<ActionResult>(
            value => Ok(value),
            err => err.Type switch
            {
                ErrorType.NotFound => NotFound(err.Description),
                _ => Problem()
            });
    }


    /// <summary>
    /// Create a command.
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateForAgent(PostCommandDTO dto, CancellationToken ct)
    {
        if (dto.TargetId == Guid.Empty)
        {
            return BadRequest($"{nameof(dto.TargetId)} is empty");
        }

        ErrorOr<Guid> commandIdResult;
        switch (dto.VoyagerTargetType)
        {
            case VoyagerTargetType.SingleHost:
                commandIdResult = await _commandManagementService.CreateCommandForAgentAsync(dto.TargetId, dto.VoyagerCommandType, "", ct);
                break;

            case VoyagerTargetType.GroupHosts:
                commandIdResult = await _commandManagementService.CreateCommandForGroupAsync(dto.TargetId, dto.VoyagerCommandType, "", ct);
                break;

            case VoyagerTargetType.AllHosts:
                commandIdResult = await _commandManagementService.CreateCommandForTenantAsync(dto.TargetId, dto.VoyagerCommandType, "", ct);
                break;

            default:
                return BadRequest($"Invalid {dto.VoyagerTargetType}");
        }

        return commandIdResult.MatchFirst<ActionResult>(
            onValue => CreatedAtAction(nameof(GetCommandById), new { id = onValue }, null),
            onError => onError.Type switch
            {
                ErrorType.NotFound => NotFound(onError.Description),
                _ => Problem()
            });
    }

}
