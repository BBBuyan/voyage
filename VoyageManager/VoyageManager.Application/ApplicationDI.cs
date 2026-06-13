using Microsoft.Extensions.DependencyInjection;
using VoyageManager.Application.Agents;
using VoyageManager.Application.Interfaces;
using VoyageManager.Application.Mappings;
using VoyageManager.Application.Services;

namespace VoyageManager.Application;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAgentService, AgentService>();
        services.AddScoped<IAgentManagementService, AgentManagementService>();
        services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfile).Assembly);

        return services;
    }
}
