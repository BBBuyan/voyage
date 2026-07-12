using System;

namespace VoyageManager.Application.Abstractions;

public interface IWorkerTokenProvider
{
    string GenerateJwtToken(Guid voyagerHostId, int expirationInSeconds);
}
