using System;
using System.Collections.Generic;
using VoyageManager.Conventions.Enums;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.Agents;

public static class CommandStatusHelper
{
    private static readonly Dictionary<VoyagerCommandStatus, VoyagerCommandStatus[]> AllowedTransitions = new()
    {
        [VoyagerCommandStatus.Pending] = [VoyagerCommandStatus.InProgress],

        [VoyagerCommandStatus.InProgress] =
        [
            VoyagerCommandStatus.Succeeded,
            VoyagerCommandStatus.Failed,
            VoyagerCommandStatus.Cancelled
        ],

        [VoyagerCommandStatus.Succeeded] = [],
        [VoyagerCommandStatus.Failed] = [],
        [VoyagerCommandStatus.Cancelled] = [],
    };

    public static bool CanTransition(VoyagerCommandStatus currentStatus, VoyagerCommandStatus newStatus)
    {
        if (AllowedTransitions.TryGetValue(currentStatus, out VoyagerCommandStatus[]? transitions))
        {
            return transitions.Contains(newStatus);
        }
        else
        {
            return false;
        }
    }

    public static VoyagerCommandStatus MapStatus(ConventionCommandStatus status)
    {
        return status switch
        {
            ConventionCommandStatus.Succeeded => VoyagerCommandStatus.Succeeded,
            ConventionCommandStatus.Failed => VoyagerCommandStatus.Failed,
            ConventionCommandStatus.InProgress => VoyagerCommandStatus.InProgress,
            ConventionCommandStatus.Cancelled => VoyagerCommandStatus.Cancelled,

            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static string TransitionErrorDescription(VoyagerCommandStatus currentStatus, VoyagerCommandStatus newStatus)
    {
        return $"Invalid transition from {currentStatus} to {newStatus}";
    }
}
