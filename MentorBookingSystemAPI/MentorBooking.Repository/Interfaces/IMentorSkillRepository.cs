using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IMentorSkillRepository
{
    Task<bool> AddMentorSkillAsync(MentorSkill mentorSkill);
}