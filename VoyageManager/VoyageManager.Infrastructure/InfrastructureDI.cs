using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoyageManager.Application.Abstractions;
using VoyageManager.Infrastructure.Database;
using VoyageManager.Infrastructure.Database.Repositories;
using VoyageManager.Infrastructure.Identity;

namespace VoyageManager.Infrastructure;

public static class InfrastructureDI
{
    /// <summary>
    /// Add dependencies that are related to the infrastructure.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VoyageManagerDbContext>(opts =>
        {
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IWorkerRespository, AgentRepository>();
        services.AddScoped<IAgentManagementRepository, AgentManagementRepository>();
        services.AddScoped<ICommandManagementRepository, CommandManagementRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IWorkerPasswordHasher, VoyagePasswordHasher>();
        services.AddScoped<IWorkerTokenProvider, WorkerTokenProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
