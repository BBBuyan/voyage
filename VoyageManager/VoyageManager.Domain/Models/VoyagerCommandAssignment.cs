using System;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Domain.Models;

public class VoyagerCommandAssignment
{
    public Guid Id { get; set; }

    public Guid VoyagerAgentId { get; set; }
    public VoyagerAgent VoyagerAgent { get; set; } = null!;

    public Guid VoyagerCommandId { get; set; }
    public VoyagerCommand VoyagerCommand { get; set; } = null!;

    /// <summary>
    /// The agents should report the progress/status of the command 
    /// they are executing.
    /// </summary>
    public VoyagerCommandStatus Status { get; set; }

    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset FinishedAt { get; set; }
}
