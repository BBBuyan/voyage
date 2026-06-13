using VoyageManager.Conventions.Enums;

namespace VoyageManager.Conventions.Agents;

public class CommandStatusResponse
{
    public ConventionCommandResponseType ResponseType { get; set; }

    public static CommandStatusResponse Continue()
    {
        return new()
        {
            ResponseType = ConventionCommandResponseType.Continue,
        };
    }

    public static CommandStatusResponse Stop()
    {
        return new()
        {
            ResponseType = ConventionCommandResponseType.Stop,
        };
    }
}
