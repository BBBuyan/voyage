using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VoyageManager.Application.Abstractions;

namespace VoyageManager.Infrastructure.Identity;

public class AgentTokenProvider : IVoyageTokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly SymmetricSecurityKey _key;
    private readonly SigningCredentials _signingCredentials;

    public AgentTokenProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
        _key = new(Convert.FromBase64String(jwtOptions.Value.Key));
        _signingCredentials = new(_key, SecurityAlgorithms.HmacSha256);
    }

    public string GenerateJwtToken(Guid voyagerHostId, int expirationInSeconds)
    {
        Dictionary<string, object> claims = new()
        {
            ["sub"] = voyagerHostId,
            ["role"] = "agent"
        };

        SecurityTokenDescriptor descriptor = new()
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Expires = DateTime.UtcNow.AddSeconds(expirationInSeconds),
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = _signingCredentials,
            Claims = claims,
        };

        JsonWebTokenHandler handler = new();
        return handler.CreateToken(descriptor);
    }
}

