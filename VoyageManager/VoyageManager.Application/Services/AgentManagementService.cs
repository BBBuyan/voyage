using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using VoyageManager.Application.Abstractions;
using VoyageManager.Application.DTOs;
using VoyageManager.Application.Interfaces;

namespace VoyageManager.Application.Services;

public class AgentManagementService : IAgentManagementService
{
    private readonly IAgentManagementRepository _agentManagementRespository;
    private readonly IMapper _mapper;

    public AgentManagementService(
        IAgentManagementRepository agentManagementRespository,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _agentManagementRespository = agentManagementRespository;
    }

    public async Task<List<VoyagerAgentDTO>> GetAgents(CancellationToken ct)
    {
        return _mapper.Map<List<VoyagerAgentDTO>>(await _agentManagementRespository.GetVoyagerAgents(ct));
    }

    public async Task<List<VoyagerAgentDTO>> GetAgentsByGroupId(Guid groupId, CancellationToken ct)
    {
        return _mapper.Map<List<VoyagerAgentDTO>>(await _agentManagementRespository.GetVoyagerAgents(ct));
    }

    public async Task<int> EnableAgentById(Guid agentId, CancellationToken ct)
    {
        return await _agentManagementRespository.EnableAgentById(agentId, ct);
    }

    public async Task<int> EnableAgentsByGroupId(Guid groupId, CancellationToken ct)
    {
        return await _agentManagementRespository.EnableAgentsByGroupId(groupId, ct);
    }
}
