using System;

namespace VoyageManager.Application.Abstractions;

public interface IVoyageTokenProvider
{
    string GenerateJwtToken(Guid voyagerHostId);
}
