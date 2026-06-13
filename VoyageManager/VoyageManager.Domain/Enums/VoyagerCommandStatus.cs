namespace VoyageManager.Domain.Enums;

public enum VoyagerCommandStatus
{
    Pending = 0,
    InProgress = 1,
    Failed = 2,
    Succeeded = 3,
    /// <summary>
    /// Cancelled must be set by the management-api.
    /// </summary>
    Cancelled = 4,
}
