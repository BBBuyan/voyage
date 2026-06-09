using System;
using System.Collections.Generic;

namespace VoyageManager.Domain.Models;

public class VoyagerGroup
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    public ICollection<VoyagerGroupAssignment> GroupAssignments { get; set; } = [];
}
