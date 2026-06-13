using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        builder.Logging.AddConsole();

        builder.Services.AddHostedService<VoyagerAgentWorker>();

        var host = builder.Build();

        host.Run();
    }
}
