using VoyageManager.Conventions.Enums;

namespace VoyageManager.Conventions.Agents;

public class CommandAssignment
{
    public Guid CommandId { get; set; }

    public VoyagerCommandType CommandType { get; set; }
}
