using System;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.DTOs;

public class VoyagerCommandAssignmentDTO
{
    public Guid Id { get; set; }
    public Guid VoyagerAgentId { get; set; }
    public Guid VoyagerCommandId { get; set; }
    public VoyagerCommandStatus Status { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset FinishedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
