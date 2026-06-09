using VoyageManager.Conventions.Enums;

namespace VoyageManager.Conventions.Agents;

public class ResultsRequest
{
    public Guid CommandId { get; set; }

    public VoyagerCommandStatus CommandStatus { get; set; }
}
