using System;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.DTOs;

public class VoyagerCommandDTO
{
    public Guid Id { get; set; }

    public VoyagerCommandType CommandType { get; set; }

    public VoyagerTargetType TargetType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }
}
