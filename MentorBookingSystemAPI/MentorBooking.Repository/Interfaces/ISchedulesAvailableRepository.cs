using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface ISchedulesAvailableRepository
{
    Task<bool> AddSchedulesAvailableAsync(List<SchedulesAvailable> schedulesAvailable);
    List<SchedulesAvailable>? GetAllSchedulesAvailable(Guid mentorId);
    Task<bool> UpdateSchedulesAvailableAsync(Guid scheduleAvailableId, SchedulesAvailable schedulesAvailable);
    Task<bool> DeleteSchedulesAvailableAsync(Guid schedulesAvailableId);
    Task<SchedulesAvailable?> GetSchedulesAvailableAsync(Guid schedulesAvailableId);
}