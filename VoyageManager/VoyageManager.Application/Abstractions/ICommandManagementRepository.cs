using System;
using System.Threading.Tasks;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Abstractions;

public interface ICommandManagementRepository
{
    Task CreateCommandForAgent(Guid agentId, VoyagerCommand command);

    Task CreateCommandForGroup(Guid groupId, VoyagerCommand command);

    Task CreateCommandForAll(VoyagerCommand command);
}
