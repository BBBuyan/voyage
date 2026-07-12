using System;
using System.Collections.Generic;

namespace VoyageManager.Domain.Models;

public class Worker
{
    public Guid Id { get; set; }

    /// <summary>
    /// MachineId and SMBiosUUID combined
    /// </summary>
    public required string HardwareId { get; set; }

    public required string Name { get; set; }

    public required string PasswordHash { get; set; }

    public bool IsEnabled { get; set; }

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    public ICollection<GroupAssignment> GroupAssignments { get; set; } = [];

    public ICollection<CommandAssignment> CommandAssignments { get; set; } = [];
}
