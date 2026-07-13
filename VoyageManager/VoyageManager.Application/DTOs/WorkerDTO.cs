using System;

namespace VoyageManager.Application.DTOs;

public class WorkerDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public bool IsOnline { get; set; }

    public bool IsEnabled { get; set; }
}
