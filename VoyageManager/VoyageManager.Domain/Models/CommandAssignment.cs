using System;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Domain.Models;

public class CommandAssignment
{
    public Guid Id { get; set; }

    public WorkerCommandType CommandType { get; set; }

    public Guid WorkerId { get; set; }
    public Worker AssignedWorker { get; set; } = null!;

    /// <summary>
    /// The agents should report the progress/status of the command
    /// they are executing.
    /// </summary>
    public AssignmentState State { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? StartedAt { get; set; }

    public DateTimeOffset? FinishedAt { get; set; }

    public DateTimeOffset? CancelledAt { get; set; }

    /// <summary>
    /// Can be created by user or scheduler.
    /// </summary>
    public string? CreatedBy { get; set; }
}
