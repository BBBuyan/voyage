using System;
using System.Threading;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface ITenantRepository
{
    Task<Tenant?> GetTenantById(Guid tenantId, CancellationToken ct);
}
