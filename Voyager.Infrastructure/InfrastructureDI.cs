using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Voyager.Application.Abstractions;
using Voyager.Infrastructure.Clients;
using Voyager.Infrastructure.Storage;

namespace Voyager.Infrastructure;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IVoyageManagerClient, VoyageManagerClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7216/api/agents/");
        });

        services.AddSingleton<IAgentCredentialsStore, AgentCredentialsStore>();
        services.AddSingleton<IEnrollmentCredentialsStore, EnrollmentCredentialsStore>();
        return services;
    }
}
