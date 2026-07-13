using Microsoft.Extensions.DependencyInjection;
using VoyageManager.Application.Interfaces;
using VoyageManager.Application.Mappings;
using VoyageManager.Application.Services;
using VoyageManager.Application.Workers;

namespace VoyageManager.Application;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IWorkerService, WorkerService>();
        services.AddScoped<IWorkerManagementService, WorkerManagementService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfile).Assembly);

        return services;
    }
}
