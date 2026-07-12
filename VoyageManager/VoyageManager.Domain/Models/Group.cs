using System;
using System.Collections.Generic;

namespace VoyageManager.Domain.Models;

public class Group
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    public ICollection<GroupAssignment> GroupAssignments { get; set; } = [];
}
