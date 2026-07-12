using System;
using System.Collections.Generic;
using VoyageManager.Conventions.Enums;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.Agents;

public static class CommandStatusHelper
{
    private static readonly Dictionary<AssignmentStatus, AssignmentStatus[]> AllowedTransitions = new()
    {
        [AssignmentStatus.Pending] = [AssignmentStatus.InProgress],

        [AssignmentStatus.InProgress] =
        [
            AssignmentStatus.Done,
            AssignmentStatus.Failed,
            AssignmentStatus.Cancelled
        ],

        [AssignmentStatus.Done] = [],
        [AssignmentStatus.Failed] = [],
        [AssignmentStatus.Cancelled] = [],
    };

    public static bool CanTransition(AssignmentStatus currentStatus, AssignmentStatus newStatus)
    {
        if (AllowedTransitions.TryGetValue(currentStatus, out AssignmentStatus[]? transitions))
        {
            return transitions.Contains(newStatus);
        }
        else
        {
            return false;
        }
    }

    public static AssignmentStatus MapStatus(ConventionCommandStatus status)
    {
        return status switch
        {
            ConventionCommandStatus.Succeeded => AssignmentStatus.Done,
            ConventionCommandStatus.Failed => AssignmentStatus.Failed,
            ConventionCommandStatus.InProgress => AssignmentStatus.InProgress,
            ConventionCommandStatus.Cancelled => AssignmentStatus.Cancelled,

            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static string TransitionErrorDescription(AssignmentStatus currentStatus, AssignmentStatus newStatus)
    {
        return $"Invalid transition from {currentStatus} to {newStatus}";
    }
}
