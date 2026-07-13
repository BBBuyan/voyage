using System;
using VoyageManager.Conventions.Enums;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.Mappings;

public static class AssignmentStateMappings
{
    public static AssignmentState ToDomain(this ConventionAssignmentState status)
    {
        return status switch
        {
            ConventionAssignmentState.Done => AssignmentState.Done,
            ConventionAssignmentState.Failed => AssignmentState.Failed,
            ConventionAssignmentState.InProgress => AssignmentState.InProgress,
            ConventionAssignmentState.Cancelled => AssignmentState.Cancelled,

            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
        };
    }
}
