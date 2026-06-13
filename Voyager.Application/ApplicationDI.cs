using Microsoft.Extensions.DependencyInjection;
using Voyager.Application.Interfaces;
using Voyager.Application.Services;

namespace Voyager.Application;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IAgentTokenProvider, AgentTokenProvider>();
        services.AddSingleton<IAgentCredentialsProvider, AgentCredentialsProvider>();
        services.AddSingleton<IVoyagerAgentRuntime, VoyagerAgentRuntime>();
        services.AddSingleton<IAgentCommandService, AgentCommandService>();

        return services;
    }
}
