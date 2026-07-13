using System;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.DTOs;

public sealed class CreateAssignmentRequest
{
    public Guid TargetId { get; set; }

    public TargetType TargetType { get; set; }

    public WorkerCommandType CommandType { get; set; }
}
