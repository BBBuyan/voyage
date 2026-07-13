using System;
using VoyageManager.Conventions.Enums;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.Mappings;

public static class CommandTypeMapping
{
    public static ConventionCommandType ToConvention(this WorkerCommandType commandType)
    {
        return commandType switch
        {
            WorkerCommandType.InventoryScan => ConventionCommandType.InventoryScan,
            WorkerCommandType.DiscoveryScan => ConventionCommandType.DiscoveryScan,
            WorkerCommandType.ActiveDirectoryScan => ConventionCommandType.ActiveDirectoryScan,

            _ => throw new ArgumentOutOfRangeException(nameof(commandType), commandType, null),
        };
    }
}
