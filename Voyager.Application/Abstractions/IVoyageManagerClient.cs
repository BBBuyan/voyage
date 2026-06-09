using System;
using System.Threading.Tasks;
using VoyageManager.Conventions.Agents;

namespace Voyager.Application.Abstractions;

public interface IVoyageManagerClient
{
    Task<Guid> SendEnrollRequest(EnrollRequest request);

    Task CheckIn();

    Task GetToken(TokenRequest request);

    Task SendResults();
}
