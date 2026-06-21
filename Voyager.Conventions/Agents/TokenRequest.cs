namespace VoyageManager.Conventions.Agents;

public class TokenRequest
{
    public Guid AgentId { get; set; }

    public required string Password { get; set; }
}
