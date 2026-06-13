using System;

namespace Voyager.Domain.Models;

public class VoyagerAgentCredentials
{
    public Guid AgentId { get; set; }

    public Guid TenantId { get; set; }

    public required string Name { get; set; }

    public required string Password { get; set; }

    public bool IsValid()
    {
        if (TenantId == Guid.Empty)
            return false;

        if (AgentId == Guid.Empty)
            return false;

        if (string.IsNullOrEmpty(Name))
            return false;

        if (string.IsNullOrEmpty(Password))
            return false;

        return true;
    }
}
