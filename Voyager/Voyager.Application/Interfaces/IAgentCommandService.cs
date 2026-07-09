using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Interfaces;

public interface IAgentCommandService
{
    Task<AgentCommand?> GetAssignedCommandAsync(string token, CancellationToken ct = default);
    Task SendCommandStatusAsync(string token, AgentCommand command, CancellationToken ct = default);
}
