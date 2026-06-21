using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Voyager.Application.Abstractions;
using Voyager.Domain.Helpers;
using Voyager.Domain.Models;

namespace Voyager.Infrastructure.Storage;

public class AgentCredentialsStore : BaseLocalStore, IAgentCredentialsStore
{
    private readonly string _agentCredentialsFilePath;
    private readonly JsonSerializerOptions _agentJsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public AgentCredentialsStore()
    {
        _agentCredentialsFilePath = Path.Combine(_commonApplicationPath, AgentFileNames.AgentCredentialsFileName);
    }

    public async Task<VoyagerAgentCredentials?> ReadAgentCredentialsAsync(CancellationToken ct)
    {
        try
        {
            if (!File.Exists(_agentCredentialsFilePath))
                return null;

            string json = await File.ReadAllTextAsync(_agentCredentialsFilePath, ct);
            VoyagerAgentCredentials? creds = JsonSerializer.Deserialize<VoyagerAgentCredentials>(json);

            if (creds is null || !creds.IsValid())
                return null;

            return creds;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> SaveAgentCredentialsAsync(VoyagerAgentCredentials credentials, CancellationToken ct)
    {
        try
        {
            string json = JsonSerializer.Serialize(credentials, _agentJsonSerializerOptions);
            await File.WriteAllTextAsync(_agentCredentialsFilePath, json, ct);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
