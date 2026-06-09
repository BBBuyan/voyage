using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Voyager.Application;
using Voyager.Infrastructure;
using Voyager.Worker;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();

        host.Run();
    }
}
