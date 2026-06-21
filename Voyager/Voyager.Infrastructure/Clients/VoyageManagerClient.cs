using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VoyageManager.Conventions.Agents;
using VoyageManager.Conventions.Enums;
using Voyager.Application.Abstractions;
using Voyager.Domain.Enums;
using Voyager.Domain.Models;
using Voyager.Infrastructure.Generators;

namespace Voyager.Infrastructure.Clients;

public class VoyageManagerClient : IVoyageManagerClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<VoyageManagerClient> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public VoyageManagerClient(HttpClient client, ILogger<VoyageManagerClient> logger)
    {
        _httpClient = client;
        _logger = logger;
    }

    public async Task<VoyagerAgentCredentials> SendEnrollRequestAsync(EnrollmentCredentials enrollmentCreds, CancellationToken ct)
    {
        string generatedName = NameGenerator.GenerateName();
        string generatedPassword = PasswordGenerator.GeneratePassword();
        EnrollRequest request = new()
        {
            TenantId = enrollmentCreds.TenantId,
            EnrollmentSecret = enrollmentCreds.EnrollmentSecret,
            Name = generatedName,
            Password = generatedPassword
        };

        using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("enroll", request, ct);
        response.EnsureSuccessStatusCode();

        Guid agentId = await response.Content.ReadFromJsonAsync<Guid>(JsonOptions, ct);
        if (agentId == Guid.Empty)
            throw new InvalidOperationException($"Server returned invalid agent id {agentId}");

        VoyagerAgentCredentials agentCreds = new()
        {
            AgentId = agentId,
            TenantId = enrollmentCreds.TenantId,
            Name = generatedName,
            Password = generatedPassword
        };
        return agentCreds;
    }

    public async Task<AgentToken> FetchTokenAsync(VoyagerAgentCredentials creds, CancellationToken ct)
    {
        TokenRequest request = new()
        {
            AgentId = creds.AgentId,
            Password = creds.Password
        };
        using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("token", request, ct);
        response.EnsureSuccessStatusCode();

        TokenResult? result = await response.Content.ReadFromJsonAsync<TokenResult>(JsonOptions, ct);
        if (result is null)
            throw new Exception("Could not get token");

        AgentToken agentToken = new()
        {
            Value = result.AccessToken,
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(result.ExpiresIn),
        };
        return agentToken;
    }

    public async Task<AgentCommand?> CheckInAsync(string token, CancellationToken ct)
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, "check-in");
        requestMessage.Headers.Authorization = new("Bearer", token);

        using HttpResponseMessage response = await _httpClient.SendAsync(requestMessage, ct);
        response.EnsureSuccessStatusCode();

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return null;
        }

        CheckInResponse? checkInResponse = await response.Content
            .ReadFromJsonAsync<CheckInResponse>(JsonOptions, ct);
        if (checkInResponse is null)
            return null;

        AgentCommand result = new()
        {
            Id = checkInResponse.Id,
            CommandType = (AgentCommandType)checkInResponse.CommandType,
            Status = AgentCommandStatus.InProgress,
        };

        return result;
    }

    public async Task SendCommandStatus(string token, AgentCommand agentCommand, CancellationToken ct)
    {
        CommandStatusRequest requestContent = new()
        {
            CommandStatus = (ConventionCommandStatus)agentCommand.Status
        };
        using HttpRequestMessage requestMessage = new(HttpMethod.Post, $"commands/{agentCommand.Id}/status");
        requestMessage.Headers.Authorization = new("Bearer", token);
        requestMessage.Content = JsonContent.Create(requestContent, options: JsonOptions);

        using HttpResponseMessage response = await _httpClient
            .SendAsync(requestMessage, ct);
        response.EnsureSuccessStatusCode();

        CommandStatusResponse? commandStatusResponse = await response.Content
            .ReadFromJsonAsync<CommandStatusResponse>(JsonOptions, ct);
    }
}
