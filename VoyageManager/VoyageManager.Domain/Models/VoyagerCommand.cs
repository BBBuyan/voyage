using System;
using System.Collections.Generic;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Domain.Models;

public class VoyagerCommand
{
    public Guid Id { get; set; }

    public VoyagerCommandType CommandType { get; set; }

    public VoyagerTargetType TargetType { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public ICollection<VoyagerCommandAssignment> CommandAssigments { get; set; } = [];
}
