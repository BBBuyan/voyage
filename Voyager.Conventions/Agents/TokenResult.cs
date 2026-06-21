namespace VoyageManager.Conventions.Agents;

public class TokenResult
{
    public required string AccessToken { get; set; }

    /// <summary>
    /// In seconds.
    /// </summary>
    public required int ExpiresIn { get; set; }
}
