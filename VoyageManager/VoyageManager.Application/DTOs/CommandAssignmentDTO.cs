using System;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.DTOs;

public class CommandAssignmentDTO
{
    public Guid Id { get; set; }

    public Guid WorkerId { get; set; }

    public AssignmentState State { get; set; }

    public WorkerCommandType CommandType { get; set; }

    public DateTimeOffset? StartedAt { get; set; }

    public DateTimeOffset? FinishedAt { get; set; }

    public DateTimeOffset? CancelledAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }
}
