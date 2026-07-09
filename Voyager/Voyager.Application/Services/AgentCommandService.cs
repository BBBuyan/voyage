using System.Threading;
using System.Threading.Tasks;
using Voyager.Application.Abstractions;
using Voyager.Application.Interfaces;
using Voyager.Domain.Models;

namespace Voyager.Application.Services;

public class AgentCommandService : IAgentCommandService
{
    private readonly IVoyageManagerClient _voyageManagerClient;

    public AgentCommandService(IVoyageManagerClient client)
    {
        _voyageManagerClient = client;
    }

    public async Task<AgentCommand?> GetAssignedCommandAsync(string token, CancellationToken ct)
    {
        return await _voyageManagerClient.CheckInAsync(token, ct);
    }

    public async Task SendCommandStatusAsync(
        string token,
        AgentCommand command,
        CancellationToken ct
    )
    {
        await _voyageManagerClient.SendCommandStatus(token, command, ct);
    }

    public async Task ExecuteCommandAsync(AgentCommand command)
    {

    }
}
