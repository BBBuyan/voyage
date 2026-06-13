using VoyageManager.Conventions.Enums;

namespace VoyageManager.Conventions.Agents;

public class CheckInResponse
{
    public Guid Id { get; set; }

    public ConventionCommandType CommandType { get; set; }
}
