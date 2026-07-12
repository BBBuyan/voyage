using System;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Domain.Models;

public class CommandAssignment
{
    public Guid Id { get; set; }

    public CommandType Type { get; set; }

    public Guid WorkerId { get; set; }
    public Worker AssignedWorker { get; set; } = null!;

    /// <summary>
    /// The agents should report the progress/status of the command 
    /// they are executing.
    /// </summary>
    public AssignmentStatus Status { get; set; }

    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Can be created by user or scheduler.
    /// </summary>
    public string? CreatedBy { get; set; }
}
