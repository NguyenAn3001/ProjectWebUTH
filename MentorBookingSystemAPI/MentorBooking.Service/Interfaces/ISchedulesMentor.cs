using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.Interfaces;

public interface ISchedulesMentor
{
    Task<ApiResponse> AddSchedulesAsync(Guid mentorId, List<SchedulesAvailableModelRequest> schedulesModels);
    ApiResponse GetSchedules(Guid mentorId);
    Task<ApiResponse> UpdateScheduleAsync(Guid mentorId, Guid scheduleAvailableId, SchedulesAvailableModelRequest scheduleModels);
    Task<ApiResponse> DeleteScheduleAsync(Guid scheduleId);
}