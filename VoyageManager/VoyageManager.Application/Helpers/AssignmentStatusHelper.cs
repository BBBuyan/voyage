using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.Helpers;

public static class AssignmentStatusHelper
{
    public static string TransitionErrorDescription(
        AssignmentState currentStatus,
        AssignmentState newStatus
    )
    {
        return $"Invalid transition from {currentStatus} to {newStatus}";
    }
}
