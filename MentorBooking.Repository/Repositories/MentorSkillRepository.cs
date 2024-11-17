using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Repository.Repositories;

public class MentorSkillRepository : IMentorSkillRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MentorSkillRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> AddMentorSkillAsync(MentorSkill mentorSkill)
    {
        try
        {
            var existingMentorSkill = await _dbContext.MentorSkills.FirstOrDefaultAsync(x => x.MentorId == mentorSkill.MentorId && x.SkillId == mentorSkill.SkillId);
            if (existingMentorSkill == null)
            {
                await _dbContext.MentorSkills.AddAsync(mentorSkill);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public List<MentorSkill> GetMentorSkillByIdAsync(Guid MentorId)
    {
        var listMentorskill = _dbContext.MentorSkills.Where(temp=>temp.MentorId==MentorId).ToList();
        return listMentorskill;
    }
}