using System.ComponentModel.DataAnnotations;

namespace VoyageManager.Infrastructure.Identity;

public class JwtOptions
{
    public const string SECTION_NAME = "Jwt";

    [Required(AllowEmptyStrings = false)]
    public required string Issuer { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string Audience { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string Key { get; set; }
}
