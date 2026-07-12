using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using VoyageManager.Application.DTOs;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.Services;

public class GroupManagementService
{
    public async Task<ErrorOr<List<VoyagerWorkerDTO>>> GetAgentsByGroupId(Guid groupId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<Guid>> CreateCommandAssignmentAsync(
        Guid groupId,
        CommandType type,
        string? username,
        CancellationToken ct
        )
    {
        throw new NotImplementedException();
    }

    public async Task<int> EnableGroupMembersAsync(Guid groupId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<Success>> CancelAssignmentExecutionAsync(Guid groupId, Guid assignmentId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
