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

namespace VoyageManager.Application.Services;

public class WorkerManagementService : IWorkerManagementService
{
    private readonly IAgentManagementRepository _agentManagementRespository;
    private readonly IMapper _mapper;

    public WorkerManagementService(
        IAgentManagementRepository agentManagementRespository,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _agentManagementRespository = agentManagementRespository;
    }

    public async Task<ErrorOr<Guid>> CreateAssignmentAsync(
        Guid agentId,
        CommandType type,
        string? username,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<List<VoyagerWorkerDTO>> GetAgentsAsync(CancellationToken ct)
    {
        return _mapper.Map<List<VoyagerWorkerDTO>>(await _agentManagementRespository.GetVoyagerAgents(ct));
    }

    public async Task<int> EnableAgentAsync(Guid agentId, CancellationToken ct)
    {
        return await _agentManagementRespository.EnableAgentById(agentId, ct);
    }

    public async Task<int> DisableAgentAsync(Guid agentId, CancellationToken ct)
    {
        return await _agentManagementRespository.EnableAgentById(agentId, ct);
    }

    public async Task<ErrorOr<Success>> CancelAssignmentExecutionAsync(Guid agentId, Guid assignmentId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
