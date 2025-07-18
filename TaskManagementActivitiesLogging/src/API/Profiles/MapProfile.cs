using Application.DTO.ActivityLog;
using AutoMapper;
using Domain.Entities;

namespace API.Profiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<CreateActivityLogRequest, ActivityLog>();
            CreateMap<ActivityLogConsumerMessage, ActivityLog>();
            CreateMap<ActivityLog, ActivityLogResponse>();
        }
    }
}
