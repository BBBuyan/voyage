namespace VoyageManager.Conventions.Agents;

public class CheckInResponse
{
    public bool Shutdown { get; set; }

    public bool CancelCurrentAssignment { get; set; }

    public ConventionCommandAssignment? CommandAssignment { get; set; }
}
