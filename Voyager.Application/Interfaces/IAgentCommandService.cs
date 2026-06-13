using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Interfaces;

public interface IAgentCommandService
{
    Task<List<AgentCommand>> GetAssignedCommands(string token, CancellationToken ct);
    Task SendCommandStatusAsync(string token, AgentCommand command, CancellationToken ct);
}
