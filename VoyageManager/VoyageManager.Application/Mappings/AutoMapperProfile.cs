using AutoMapper;
using VoyageManager.Application.Results;
using VoyageManager.Conventions.Agents;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Mappings;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<VoyagerAgent, VoyagerAgentDTO>();
        CreateMap<VoyagerCommandAssignment, CommandAssignment>();
    }
}
