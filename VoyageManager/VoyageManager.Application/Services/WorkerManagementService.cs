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
    private readonly IWorkerManagementRepository _workerManagementRespository;
    private readonly IMapper _mapper;

    public WorkerManagementService(
        IWorkerManagementRepository agentManagementRespository,
        IMapper mapper
    )
    {
        _mapper = mapper;
        _workerManagementRespository = agentManagementRespository;
    }

    public async Task<ErrorOr<Guid>> CreateAssignmentAsync(
        Guid agentId,
        WorkerCommandType type,
        string? username,
        CancellationToken ct
    )
    {
        throw new NotImplementedException();
    }

    public async Task<List<WorkerDTO>> GetAgentsAsync(CancellationToken ct)
    {
        return _mapper.Map<List<WorkerDTO>>(await _workerManagementRespository.GetAsync(ct));
    }

    public async Task<int> EnableAgentAsync(Guid agentId, CancellationToken ct)
    {
        return await _workerManagementRespository.EnableAgentById(agentId, ct);
    }

    public async Task<int> DisableAgentAsync(Guid agentId, CancellationToken ct)
    {
        return await _workerManagementRespository.EnableAgentById(agentId, ct);
    }

    public async Task<ErrorOr<Success>> CancelAssignmentExecutionAsync(
        Guid agentId,
        Guid assignmentId,
        CancellationToken ct
    )
    {
        throw new NotImplementedException();
    }
}
