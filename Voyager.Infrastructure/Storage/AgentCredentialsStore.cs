using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Voyager.Application.Abstractions;
using Voyager.Domain.Models;

namespace Voyager.Infrastructure.Storage;

public class AgentCredentialsStore : BaseLocalStore, IAgentCredentialsStore
{
    private readonly string _agentStateFilePath;
    private readonly string _agentStateFileName = "agent-state.json";

    private readonly JsonSerializerOptions _agentJsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public AgentCredentialsStore()
    {
        _agentStateFilePath = Path.Combine(_commonApplicationPath, _agentStateFileName);
    }

    public async Task<VoyagerAgentCredentials?> ReadAgentCredentials(CancellationToken ct)
    {
        try
        {
            if (!File.Exists(_agentStateFilePath))
                return null;
            string json = await File.ReadAllTextAsync(_agentStateFilePath, ct);

            if (string.IsNullOrWhiteSpace(json))
                return null;

            VoyagerAgentCredentials? agentState = JsonSerializer.Deserialize<VoyagerAgentCredentials>(json);

            if (agentState is null)
                return null;

            if (agentState.TenantId == Guid.Empty)
                return null;

            if (agentState.AgentId == Guid.Empty)
                return null;

            if (string.IsNullOrEmpty(agentState.Name))
                return null;

            if (string.IsNullOrEmpty(agentState.Password))
                return null;

            return agentState;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> SaveAgentCredentials(VoyagerAgentCredentials credentials, CancellationToken ct)
    {
        try
        {
            string json = JsonSerializer.Serialize(credentials, _agentJsonSerializerOptions);
            await File.WriteAllTextAsync(_agentStateFilePath, json, ct);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
