using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using VoyageManager.Application.Abstractions;
using VoyageManager.Application.DTOs;
using VoyageManager.Application.Interfaces;
using VoyageManager.Domain.Enums;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Services;

public class CommandManagementService : ICommandManagementService
{
    private readonly ICommandManagementRepository _commandManagementRepository;
    private readonly IAgentManagementRepository _agentManagementRepository;
    private readonly IMapper _mapper;

    public CommandManagementService(
        ICommandManagementRepository commandManagementRepository,
        IAgentManagementRepository agentManagementRepository,
        IMapper mapper
        )
    {
        _commandManagementRepository = commandManagementRepository;
        _agentManagementRepository = agentManagementRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Guid>> CreateCommandForAgentAsync(
        Guid agentId,
        VoyagerCommandType type,
        string? username,
        CancellationToken ct)
    {
        bool exists = await _agentManagementRepository.AgentExistsAsync(agentId, ct);
        if (!exists)
        {
            return Error.NotFound(description: "Agent not found.");
        }
        VoyagerCommandAssignment assignment = new()
        {
            VoyagerAgentId = agentId,
        };
        VoyagerCommand command = new()
        {
            CommandType = type,
            TargetType = VoyagerTargetType.SingleHost,
            CreatedBy = username,
        };
        command.CommandAssigments.Add(assignment);
        await _commandManagementRepository.CreateCommandAsync(command, ct);
        return command.Id;
    }

    public async Task<ErrorOr<Guid>> CreateCommandForGroupAsync(
        Guid groupId,
        VoyagerCommandType type,
        string? username,
        CancellationToken ct)
    {
        List<Guid> agentIds = await _agentManagementRepository.GetVoyagerAgentIdsByGroupId(groupId, ct);
        List<VoyagerCommandAssignment> assignments = new();
        foreach (Guid agentId in agentIds)
        {
            assignments.Add(new() { VoyagerAgentId = agentId });
        }
        VoyagerCommand command = new()
        {
            CommandType = type,
            TargetType = VoyagerTargetType.GroupHosts,
            CreatedBy = username,
            CommandAssigments = assignments
        };
        await _commandManagementRepository.CreateCommandAsync(command, ct);
        return command.Id;
    }

    public async Task<ErrorOr<Guid>> CreateCommandForTenantAsync(
        Guid tenantId,
        VoyagerCommandType type,
        string? username,
        CancellationToken ct)
    {
        List<Guid> agentIds = await _agentManagementRepository.GetVoyagerAgentIdsByTenantId(tenantId, ct);
        List<VoyagerCommandAssignment> assignments = new();
        foreach (Guid id in agentIds)
        {
            assignments.Add(new() { VoyagerAgentId = id });
        }
        VoyagerCommand command = new()
        {
            CommandType = type,
            TargetType = VoyagerTargetType.GroupHosts,
            CreatedBy = username,
            CommandAssigments = assignments
        };
        await _commandManagementRepository.CreateCommandAsync(command, ct);
        return command.Id;
    }

    public async Task<ErrorOr<List<VoyagerCommandAssignmentDTO>>> GetCommandAssignmentsByCommandId(Guid commandId, CancellationToken ct)
    {
        List<VoyagerCommandAssignment> result = await _commandManagementRepository.GetVoyagerCommandAssignmentsByCommandIdAsync(commandId, ct);
        return _mapper.Map<List<VoyagerCommandAssignmentDTO>>(result);
    }

    public async Task<ErrorOr<VoyagerCommandDTO>> GetCommandByIdAsync(Guid commandId, CancellationToken ct)
    {
        VoyagerCommand? command = await _commandManagementRepository.GetVoyagerCommandByIdAsync(commandId, ct);
        if (command == null)
            return Error.NotFound(description: "VoyagerCommand Not Found.");

        return _mapper.Map<VoyagerCommandDTO>(command);
    }

    public async Task<ErrorOr<List<VoyagerCommandDTO>>> GetCommandsAsync(CancellationToken ct)
    {
        List<VoyagerCommand> result = await _commandManagementRepository.GetVoyagerCommandsAsync(ct);
        return _mapper.Map<List<VoyagerCommandDTO>>(result);
    }
}
