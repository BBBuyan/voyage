using Microsoft.Extensions.DependencyInjection;
using Voyager.Application.Interfaces;
using Voyager.Application.Services;

namespace Voyager.Application;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IEnrollmentService, EnrollmentService>();
        return services;
    }
}
