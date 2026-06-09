using System;
using System.Collections.Generic;

namespace VoyageManager.Domain.Models;

public class VoyagerAgent
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string PasswordHash { get; set; }

    public bool IsOnline { get; set; }

    public bool IsEnabled { get; set; }

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    public ICollection<VoyagerGroupAssignment> GroupAssignments { get; set; } = [];

    public ICollection<VoyagerCommandAssignment> CommandAssignments { get; set; } = [];
}
