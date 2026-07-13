using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using VoyageManager.Infrastructure.ApiDocumentation;

namespace VoyageManager.Api.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opts =>
        {
            string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            opts.AddSecurityDefinition(
                VoyagerBearerSecurityDefinition.Name,
                VoyagerBearerSecurityDefinition.Scheme
            );
            opts.AddSecurityRequirement(doc =>
            {
                return new OpenApiSecurityRequirement()
                {
                    [
                        new OpenApiSecuritySchemeReference(
                            VoyagerBearerSecurityDefinition.Name,
                            doc
                        )
                    ] = [],
                };
            });
            opts.OperationFilter<AuthorizeOperationFilter>();
        });

        return services;
    }
}
