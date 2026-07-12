using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using VoyageManager.Application.Abstractions;
using VoyageManager.Application.Interfaces;
using VoyageManager.Domain.Enums;

namespace VoyageManager.Application.Services;

public class CommandService : ICommandService
{
    private readonly ICommandManagementRepository _commandManagementRepository;
    private readonly IAgentManagementRepository _agentManagementRepository;
    private readonly IMapper _mapper;

    public CommandService(
        ICommandManagementRepository commandManagementRepository,
        IAgentManagementRepository agentManagementRepository,
        IMapper mapper
        )
    {
        _commandManagementRepository = commandManagementRepository;
        _agentManagementRepository = agentManagementRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Guid>> CreateAssignmentAsync(
        Guid agentId,
        CommandType type,
        string? username,
        CancellationToken ct
        )
    {
        throw new NotImplementedException();
    }


}
