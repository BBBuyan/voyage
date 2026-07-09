using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Application.DTOs;

namespace VoyageManager.Api.Controllers;

[ApiController]
[Route("api/v1/groups")]
public class GroupManagementController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<VoyagerGroupDTO>> GetGroups()
    {
        return Ok(new());
    }

    [AllowAnonymous]
    [HttpGet("{id}/members")]
    public async Task<ActionResult<VoyagerAgentDTO>> GetGroupMembers(Guid Id)
    {
        return Ok(new());
    }

    [AllowAnonymous]
    [HttpGet("{id}/assignments")]
    public async Task<ActionResult<VoyagerAgentDTO>> EnableGroupAgents(Guid Id)
    {
        return Ok(new());
    }

    [AllowAnonymous]
    [HttpPost("{id}/assignments")]
    public async Task<ActionResult<VoyagerAgentDTO>> CreateGroupCommandAssignments(Guid Id)
    {
        return Ok(new());
    }

    [AllowAnonymous]
    [HttpPost("{id}/status")]
    public async Task<ActionResult<VoyagerAgentDTO>> UpdateGroupAgentStatus(Guid Id)
    {
        return Ok(new());
    }
}
