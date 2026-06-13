using System;
using Voyager.Domain.Enums;

namespace Voyager.Domain.Models;

public class AgentCommand
{
    public Guid Id { get; set; }

    public AgentCommandType CommandType { get; set; }

    public AgentCommandStatus Status { get; set; }
}
