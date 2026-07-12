namespace VoyageManager.Domain.Enums;

public enum AssignmentStatus
{
    Pending = 0,
    InProgress = 1,
    Done = 2,
    Failed = 3,
    /// <summary>
    /// Cancelled must be set by the management-api.
    /// </summary>
    Cancelled = 4,
}
