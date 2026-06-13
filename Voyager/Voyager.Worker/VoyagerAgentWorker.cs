using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Voyager.Application.Interfaces;

namespace Voyager.Worker;

public class VoyagerAgentWorker : BackgroundService
{
    private readonly IVoyagerAgentRuntime _runtime;

    public VoyagerAgentWorker(IVoyagerAgentRuntime voyagerAgentRuntime)
    {
        _runtime = voyagerAgentRuntime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _runtime.RunAsync(stoppingToken);
    }
}
