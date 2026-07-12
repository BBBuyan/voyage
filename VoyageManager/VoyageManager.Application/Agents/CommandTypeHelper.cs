using System;
using VoyageManager.Conventions.Enums;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.Agents;

public static class CommandTypeHelper
{
    public static ConventionCommandType ToConvention(this CommandType type)
    {
        return type switch
        {
            CommandType.InventoryScan => ConventionCommandType.InventoryScan,
            CommandType.DiscoveryScan => ConventionCommandType.DiscoveryScan,
            CommandType.ActiveDirectoryScan => ConventionCommandType.ActiveDirectoryScan,

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }
}
