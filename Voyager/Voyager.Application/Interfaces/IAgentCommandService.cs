using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Interfaces;

public interface IAgentCommandService
{
    Task<AgentCommand?> GetAssignedCommand(string token, CancellationToken ct);
    Task SendCommandStatusAsync(string token, AgentCommand command, CancellationToken ct);
}
