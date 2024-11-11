using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IMentorRepository
{
    Task<bool> AddInformationMentorAsync(Mentor mentor);
    Task<Mentor?> GetMentorByIdAsync(Guid MentorId);
}