using AutoMapper;
using VoyageManager.Application.DTOs;
using VoyageManager.Conventions.Enums;
using VoyageManager.Domain.Enums;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Mappings;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Worker, VoyagerWorkerDTO>();
        CreateMap<CommandType, ConventionCommandType>();
        CreateMap<CommandAssignment, VoyagerCommandAssignmentDTO>();
    }
}
