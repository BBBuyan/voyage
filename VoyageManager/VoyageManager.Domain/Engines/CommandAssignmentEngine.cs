using System;
using System.Collections.Generic;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Domain.Engines;

public static class CommandAssignmentEngine
{
    private static readonly Dictionary<AssignmentState, AssignmentState[]> AllowedTransitions =
        new()
        {
            [AssignmentState.Pending] =
            [
                AssignmentState.InProgress,
                AssignmentState.CancelRequested,
            ],

            [AssignmentState.InProgress] =
            [
                AssignmentState.Done,
                AssignmentState.Failed,
                AssignmentState.CancelRequested,
            ],

            [AssignmentState.CancelRequested] =
            [
                AssignmentState.Done,
                AssignmentState.Failed,
                AssignmentState.Cancelled,
            ],

            [AssignmentState.Done] = [],
            [AssignmentState.Failed] = [],
            [AssignmentState.Cancelled] = [],
        };

    public static bool CanTransition(AssignmentState currentStatus, AssignmentState newStatus)
    {
        if (AllowedTransitions.TryGetValue(currentStatus, out AssignmentState[]? transitions))
        {
            return transitions.Contains(newStatus);
        }
        else
        {
            return false;
        }
    }
}
