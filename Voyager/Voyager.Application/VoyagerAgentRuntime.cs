using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voyager.Application.Interfaces;
using Voyager.Domain.Enums;
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
        VoyagerAgentCredentials agentCredentials =
            await _agentCredentialsProvider.GetAgentCredentialsAsync(ct);

        while (!ct.IsCancellationRequested)
        {
            AgentToken agentToken = await _agentTokenProvider.GetTokenAsync(agentCredentials, ct);
            AgentCommand? assignedCommand = await _agentCommandService
                .GetAssignedCommandAsync(agentToken.Value, ct);

            if (assignedCommand != null)
            {
                try
                {
                    _logger.LogInformation(
                        "Executing {CommandId}, {CommandType}",
                        assignedCommand.Id,
                        assignedCommand.CommandType.ToString()
                    );
                    // Command Logic
                    assignedCommand.Status = AgentCommandStatus.Succeeded;
                }
                catch (Exception ex)
                {
                    _logger.LogError(message: ex.Message);
                }
                await _agentCommandService.SendCommandStatusAsync(
                    agentToken.Value,
                    assignedCommand,
                    ct
                );
            }

            await Task.Delay(5000, ct);
        }
    }
}
