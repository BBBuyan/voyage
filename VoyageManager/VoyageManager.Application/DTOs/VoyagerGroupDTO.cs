using System;

namespace VoyageManager.Application.DTOs;

public class VoyagerGroupDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
}
