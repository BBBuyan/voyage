using System.Threading;
using System.Threading.Tasks;

namespace Voyager.Application.Interfaces;

public interface IVoyagerAgentRuntime
{
    Task RunAsync(CancellationToken ct);
}
