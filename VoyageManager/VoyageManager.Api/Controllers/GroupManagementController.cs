using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoyageManager.Api.Controllers.Base;
using VoyageManager.Application.DTOs;

namespace VoyageManager.Api.Controllers;

[ApiController]
[Route("api/v1/groups")]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class GroupManagementController : BaseController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<VoyagerGroupDTO>> GetGroups()
    {
        return Ok(new());
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<VoyagerWorkerDTO>> CreateGroup(CancellationToken ct)
    {
        return Ok(new());
    }

    [AllowAnonymous]
    [HttpDelete]
    public async Task<ActionResult<VoyagerWorkerDTO>> DeleteGroup(Guid id, CancellationToken ct)
    {
        return Ok(new());
    }

    [AllowAnonymous]
    [HttpGet("{id}/members")]
    public async Task<ActionResult<VoyagerWorkerDTO>> GetGroupMembers(Guid id, CancellationToken ct)
    {
        return Ok(new());
    }

    [AllowAnonymous]
    [HttpDelete("{groupId:guid}/members/{memberId:guid}")]
    public async Task<ActionResult<VoyagerWorkerDTO>> AddGroupMember(Guid groupId, Guid memberId, CancellationToken ct)
    {
        return Ok(new());
    }

    [AllowAnonymous]
    [HttpPut("{groupId:guid}/members/{memberId:guid}")]
    public async Task<ActionResult<VoyagerWorkerDTO>> DeleteGroupMember(Guid groupId, Guid memberId, CancellationToken ct)
    {
        return Ok(new());
    }

}
