using AutoMapper;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<SchedulesAvailable, SchedulesAvailableModelResponse>()
            .ForMember(x => x.ScheduleAvailableId, option => option.MapFrom(src => src.ScheduleAvailableId))
            .ForMember(x => x.MentorId, option => option.MapFrom(src => src.MentorId))
            .ForMember(x => x.FreeDay, option => option.MapFrom(src => src.FreeDay))
            .ForMember(x => x.StartTime, option => option.MapFrom(src => src.StartTime))
            .ForMember(x => x.EndTime, option => option.MapFrom(src => src.EndTime));
    }
}