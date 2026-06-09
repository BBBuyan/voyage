namespace VoyageManager.Conventions.Agents;

public class EnrollRequest
{
    public required Guid TenantId { get; set; }

    public required string EnrollmentSecret { get; set; }

    public required string Name { get; set; }

    public required string Password { get; set; }
}
