using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface IWorkerManagementRepository
{
    Task<bool> ExistsAsync(Guid agentId, CancellationToken ct);

    Task<List<Worker>> GetAsync(CancellationToken ct);

    Task<List<Guid>> GetAsync(Guid tenantId, CancellationToken ct);

    Task<List<Guid>> GetByGroupId(Guid groupId, CancellationToken ct);

    Task<int> EnableAgentById(Guid agentId, CancellationToken ct);

    Task<int> DisableAgentById(Guid agentId, CancellationToken ct);

    Task<int> EnableAgentsByGroupId(Guid groupId, CancellationToken ct);

    Task<int> DisableAgentsByGroupId(Guid groupId, CancellationToken ct);
}
