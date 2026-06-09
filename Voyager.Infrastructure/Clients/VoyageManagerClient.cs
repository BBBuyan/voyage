using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VoyageManager.Conventions.Agents;
using Voyager.Application.Abstractions;

namespace Voyager.Infrastructure.Clients;

public class VoyageManagerClient : IVoyageManagerClient
{
    private readonly HttpClient _httpClient;

    public VoyageManagerClient(HttpClient client)
    {
        _httpClient = client;
    }

    public async Task<Guid> SendEnrollRequest(EnrollRequest request)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("enroll", request);
        response.EnsureSuccessStatusCode();
        Guid agentId = await response.Content.ReadFromJsonAsync<Guid>();
        if (agentId == Guid.Empty)
        {
            throw new InvalidOperationException($"Server returned invalid agent id {agentId}");
        }
        return agentId;
    }

    public Task CheckIn()
    {
        throw new NotImplementedException();
    }

    public Task GetToken(TokenRequest request)
    {
        throw new NotImplementedException();
    }

    public Task SendResults()
    {
        throw new NotImplementedException();
    }
}
