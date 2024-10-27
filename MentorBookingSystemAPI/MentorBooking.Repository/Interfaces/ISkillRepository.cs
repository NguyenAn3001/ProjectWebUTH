using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface ISkillRepository
{
    // Task<List<Skill>> GetAllAsync();
    Task<int> AddSkillAsync(string skillName);
}