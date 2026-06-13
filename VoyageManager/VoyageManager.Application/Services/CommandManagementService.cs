using System;
using System.Threading.Tasks;
using VoyageManager.Application.Abstractions;

namespace VoyageManager.Application.Services;

public class CommandManagementService
{
    private readonly ICommandManagementRepository _commandManagementRepository;

    public CommandManagementService(ICommandManagementRepository commandManagementRepository)
    {
        _commandManagementRepository = commandManagementRepository;
    }

    public async Task CreateCommandForAgent(Guid agentId, )
    {

    }
}
