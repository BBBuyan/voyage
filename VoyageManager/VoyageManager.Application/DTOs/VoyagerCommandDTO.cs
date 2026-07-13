using System;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.DTOs;

public class VoyagerCommandDTO
{
    public Guid Id { get; set; }

    public WorkerCommandType CommandType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }
}
