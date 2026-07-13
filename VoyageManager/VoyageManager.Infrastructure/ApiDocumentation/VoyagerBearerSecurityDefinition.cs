using Microsoft.OpenApi;

namespace VoyageManager.Infrastructure.ApiDocumentation;

public static class VoyagerBearerSecurityDefinition
{
    public const string Name = "bearer";

    public static OpenApiSecurityScheme Scheme { get; } =
        new()
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        };
}
