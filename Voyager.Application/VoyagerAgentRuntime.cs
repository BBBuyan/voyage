using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voyager.Application.Interfaces;
using Voyager.Domain.Models;

namespace Voyager.Application;

public class VoyagerAgentRuntime : IVoyagerAgentRuntime
{
    private readonly ILogger<VoyagerAgentRuntime> _logger;
    private readonly IAgentTokenProvider _agentTokenProvider;
    private readonly IAgentCredentialsProvider _agentCredentialsProvider;
    private readonly IAgentCommandService _agentCommandService;


    public VoyagerAgentRuntime(
        ILogger<VoyagerAgentRuntime> logger,
        IAgentTokenProvider agentTokenProvider,
        IAgentCredentialsProvider agentCredentialsProvider,
        IAgentCommandService agentCommandService
        )
    {
        _logger = logger;
        _agentCredentialsProvider = agentCredentialsProvider;
        _agentTokenProvider = agentTokenProvider;
        _agentCommandService = agentCommandService;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        VoyagerAgentCredentials agentCredentials = await _agentCredentialsProvider.GetAgentCredentialsAsync(ct);
        while (!ct.IsCancellationRequested)
        {
            AgentToken agentToken = await _agentTokenProvider.GetTokenAsync(agentCredentials, ct);
            _logger.LogInformation($"AgentToken {agentToken.Value}");
            List<AgentCommand> assignedCommands = await _agentCommandService.GetAssignedCommands(agentToken.Value, ct);
            _logger.LogInformation($"AssignedCommands {assignedCommands.Count}");

            foreach (AgentCommand assignedCommand in assignedCommands)
            {
                // AssignedCommand execution logic

                assignedCommand.Status = Domain.Enums.AgentCommandStatus.Succeeded;

                await _agentCommandService.SendCommandStatusAsync(agentToken.Value, assignedCommand, ct);
            }

            await Task.Delay(5000, ct);
        }
    }
}
