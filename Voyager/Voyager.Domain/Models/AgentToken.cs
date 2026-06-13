using System;

namespace Voyager.Domain.Models;

public class AgentToken
{
    public required string Value { get; set; }

    public DateTimeOffset ExpiresAt { get; init; }

    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt;
}
