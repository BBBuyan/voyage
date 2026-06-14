using AutoMapper;
using VoyageManager.Application.DTOs;
using VoyageManager.Conventions.Agents;
using VoyageManager.Conventions.Enums;
using VoyageManager.Domain.Enums;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Mappings;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<VoyagerAgent, VoyagerAgentDTO>();
        CreateMap<VoyagerCommandType, ConventionCommandType>();
        CreateMap<VoyagerCommand, CheckInResponse>();
        CreateMap<VoyagerCommand, VoyagerCommandDTO>();
        CreateMap<VoyagerCommandAssignment, VoyagerCommandAssignmentDTO>();
    }
}
