using System;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Api.DTOs;

public class PostCommandDTO
{
    public Guid TargetId { get; set; }

    public VoyagerTargetType VoyagerTargetType { get; set; }

    public VoyagerCommandType VoyagerCommandType { get; set; }
}
