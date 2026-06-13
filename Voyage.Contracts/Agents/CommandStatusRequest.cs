using VoyageManager.Conventions.Enums;

namespace VoyageManager.Conventions.Agents;

public class CommandStatusRequest
{
    public Guid CommandId { get; set; }

    public ConventionCommandStatus CommandStatus { get; set; }
}
