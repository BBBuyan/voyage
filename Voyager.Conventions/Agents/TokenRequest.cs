using System.ComponentModel.DataAnnotations;

namespace VoyageManager.Conventions.Agents;

public class TokenRequest
{
    [Required]
    public Guid AgentId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string Password { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string HardwareId { get; set; }
}
