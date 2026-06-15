namespace VoyageManager.Domain.Enums;

public enum VoyagerCommandStatus
{
    Pending = 0,
    InProgress = 1,
    Succeeded = 2,
    Failed = 3,
    /// <summary>
    /// Cancelled must be set by the management-api.
    /// </summary>
    Cancelled = 4,
}
