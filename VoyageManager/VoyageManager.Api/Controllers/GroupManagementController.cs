using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Application.DTOs;

namespace VoyageManager.Api.Controllers;

[ApiController]
[Route("api/groups")]
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
}
