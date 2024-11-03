using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces
{
    public interface IMentorWorkScheduleRepository
    {
        Task<bool> AddMentorWordScheduleAsync(MentorWorkSchedule mentorWorkSchedule);
        Task<bool> CheckWorkScheduleAsync(Guid mentorWorkScheduleId);
        Task<bool> DeleteMentorWorkScheduleAsync(Guid SessionId);
        List<MentorWorkSchedule> GetMentorWorkSchedule(Guid SessionId);
    }
}
