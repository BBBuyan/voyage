using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using VoyageManager.Application.DTOs;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.Interfaces;

public interface ICommandService
{
    Task<ErrorOr<VoyagerCommandDTO>> GetCommandByIdAsync(Guid commandId, CancellationToken ct);

    Task<ErrorOr<List<VoyagerCommandAssignmentDTO>>> GetCommandAssignmentsByCommandId(Guid commandId, CancellationToken ct);

    Task<ErrorOr<List<VoyagerCommandDTO>>> GetCommandsAsync(CancellationToken ct);

    Task<ErrorOr<Guid>> CreateCommandForAgentAsync(Guid agentId, VoyagerCommandType type, string? username, CancellationToken ct);

    Task<ErrorOr<Guid>> CreateCommandForGroupAsync(Guid groupId, VoyagerCommandType type, string? username, CancellationToken ct);

    Task<ErrorOr<Guid>> CreateCommandForTenantAsync(Guid tenantId, VoyagerCommandType type, string? username, CancellationToken ct);
}
