using System;

namespace VoyageManager.Application.Results;

public class VoyagerAgentDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public bool IsOnline { get; set; }

    public bool IsEnabled { get; set; }
}
