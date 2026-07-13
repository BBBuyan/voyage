using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using VoyageManager.Application.DTOs;

namespace VoyageManager.Application.Interfaces;

public interface IAssignmentService
{
    Task<ErrorOr<Success>> CreateAssignmentAsync(
        CreateAssignmentRequest request,
        string? username,
        CancellationToken ct
    );

    Task<ErrorOr<List<CommandAssignmentDTO>>> GetAssignmentsAsync(
        Guid? workerId,
        Guid? groupId,
        CancellationToken ct
    );

    Task<ErrorOr<Success>> CancelAssignmentAsync(Guid id, CancellationToken ct);
}
