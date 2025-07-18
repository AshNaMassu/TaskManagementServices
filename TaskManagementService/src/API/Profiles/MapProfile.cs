using Application.DTO.Logging;
using Application.DTO.Task;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTO;

namespace API.Profiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<CreateTaskRequest, TaskEntity>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.New.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<TaskEntity, TaskResponse>();

            CreateMap<EntityChangedMessage, ActivityLogMessage>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.Operation))
                .ForMember(dest => dest.Entity, opt => opt.MapFrom(src => src.Entity))
                .ForMember(dest => dest.EventTime, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<EntityChangedMessage, CreateActivityLogRequest>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.Operation))
                .ForMember(dest => dest.Entity, opt => opt.MapFrom(src => src.Entity))
                .ForMember(dest => dest.EventTime, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
