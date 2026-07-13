using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using VoyageManager.Application.DTOs;

namespace VoyageManager.Application.Services;

public class GroupManagementService
{
    public async Task<ErrorOr<List<WorkerDTO>>> GetMembersAsync(Guid groupId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<int> EnableMembersAsync(Guid groupId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
