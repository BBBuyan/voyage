using System;
using Voyager.Domain.Enums;

namespace Voyager.Domain.Models;

public abstract class AgentCommand
{
    public Guid Id { get; set; }

    public AgentCommandStatus Status { get; set; }
}
