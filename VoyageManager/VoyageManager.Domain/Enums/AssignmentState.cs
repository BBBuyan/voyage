namespace VoyageManager.Domain.Enums;

public enum AssignmentState
{
    Pending,
    InProgress,
    Done,
    Failed,

    /// <summary>
    /// Cancelled must be set by the management-api.
    /// </summary>
    CancelRequested,
    Cancelled,
}
