using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces
{
    public interface IMentorWorkScheduleRepository
    {
        Task<bool> AddMentorWordScheduleAsync(MentorWorkSchedule mentorWorkSchedule);
        Task<bool> CheckWordScheduleAsync(Guid mentorWorkScheduleId);
    }
}
