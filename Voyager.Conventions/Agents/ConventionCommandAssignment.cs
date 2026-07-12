using VoyageManager.Conventions.Enums;

namespace VoyageManager.Conventions.Agents;

public class ConventionCommandAssignment
{
    public Guid Id { get; set; }

    public Guid WorkerId { get; set; }

    public ConventionCommandType CommandType { get; set; }
}
