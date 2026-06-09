using System;
using System.Collections.Generic;
using VoyageManager.Conventions.Enums;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Domain.Models;

public class VoyagerCommand
{
    public Guid Id { get; set; }

    public VoyagerCommandType CommandType { get; set; }

    public VoyagerTargetType TargetType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public ICollection<VoyagerCommandAssignment> CommandAssigments { get; set; } = [];
}
