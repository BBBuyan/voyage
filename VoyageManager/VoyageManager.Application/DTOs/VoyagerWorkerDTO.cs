using System;

namespace VoyageManager.Application.DTOs;

public class VoyagerWorkerDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public bool IsOnline { get; set; }

    public bool IsEnabled { get; set; }
}
