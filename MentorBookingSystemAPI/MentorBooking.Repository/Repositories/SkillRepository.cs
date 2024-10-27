using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;

namespace MentorBooking.Repository.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SkillRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<int> AddSkillAsync(string skillName)
    {
        try
        {
            var existingSkill = _dbContext.Skills.FirstOrDefault(x => x.Name == skillName);
            if (existingSkill == null)
            {
                existingSkill = new Skill()
                {
                    Name = skillName
                };
                await _dbContext.Skills.AddAsync(existingSkill);
                await _dbContext.SaveChangesAsync();
            }
            return existingSkill.SkillId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return -1;
        }
    }
}