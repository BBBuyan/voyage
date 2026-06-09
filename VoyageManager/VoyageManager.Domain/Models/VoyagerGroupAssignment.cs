using System;

namespace VoyageManager.Domain.Models;

public class VoyagerGroupAssignment
{
    public Guid Id { get; set; }

    public Guid VoyagerAgentId { get; set; }
    public VoyagerAgent VoyagerAgent { get; set; } = null!;

    public Guid VoyagerGroupId { get; set; }
    public VoyagerGroup VoyagerGroup { get; set; } = null!;
}
