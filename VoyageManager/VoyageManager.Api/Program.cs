using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VoyageManager.Api.Extensions;
using VoyageManager.Application;
using VoyageManager.Infrastructure;
using VoyageManager.Infrastructure.Identity;

namespace VoyageManager.Api;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder
            .Services.AddOptions<JwtOptions>()
            .Bind(builder.Configuration.GetSection(JwtOptions.SECTION_NAME))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        JwtOptions jwtOption = builder
            .Configuration.GetSection(JwtOptions.SECTION_NAME)
            .Get<JwtOptions>()!;

        builder
            .Services.AddAuthorizationBuilder()
            .AddPolicy("WorkerOnly", policy => policy.RequireRole("worker"));

        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    RoleClaimType = "role",
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Convert.FromBase64String(jwtOption.Key)
                    ),
                };

                opts.MapInboundClaims = false;
            });

        builder
            .Services.AddControllers()
            .AddJsonOptions(opts =>
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );

        builder.Services.AddCustomSwagger();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers().RequireAuthorization();

        app.Run();
    }
}
