using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Application.DTOs;

namespace VoyageManager.Application.Interfaces;

public interface IWorkerManagementService
{
    Task<List<VoyagerWorkerDTO>> GetAgentsAsync(CancellationToken ct);

    Task<int> EnableAgentAsync(Guid agentId, CancellationToken ct);
}
