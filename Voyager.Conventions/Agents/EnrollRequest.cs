using System.ComponentModel.DataAnnotations;

namespace VoyageManager.Conventions.Agents;

public class EnrollRequest
{
    public required Guid TenantId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string EnrollmentSecret { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string Name { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string Password { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string HardwareId { get; set; }
}
