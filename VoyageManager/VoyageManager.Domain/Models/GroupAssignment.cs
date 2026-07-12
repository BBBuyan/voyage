using System;

namespace VoyageManager.Domain.Models;

public class GroupAssignment
{
    public Guid Id { get; set; }

    public Guid WorkerId { get; set; }
    public Worker AssignedWorker { get; set; } = null!;

    public Guid GroupId { get; set; }
    public Group AssignedGroup { get; set; } = null!;
}
