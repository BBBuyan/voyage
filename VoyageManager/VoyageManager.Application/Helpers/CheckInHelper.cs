using VoyageManager.Application.Mappings;
using VoyageManager.Conventions.Agents;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Helpers;

public static class CheckInHelper
{
    public static CheckInResponse ContinueResponse()
    {
        return new() { };
    }

    public static CheckInResponse ShutdownResponse()
    {
        return new() { Shutdown = true };
    }

    public static CheckInResponse CancelResponse()
    {
        return new() { CancelCurrentAssignment = true };
    }

    public static CheckInResponse AssignmentResponse(CommandAssignment assignment)
    {
        return new()
        {
            CommandAssignment = new()
            {
                Id = assignment.Id,
                CommandType = assignment.CommandType.ToConvention(),
            },
        };
    }
}
