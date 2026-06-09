using System;

namespace Voyager.Domain.Models;

public class VoyagerAgentCredentials
{
    public Guid AgentId { get; set; }

    public Guid TenantId { get; set; }

    public required string Name { get; set; }

    public required string Password { get; set; }
}
