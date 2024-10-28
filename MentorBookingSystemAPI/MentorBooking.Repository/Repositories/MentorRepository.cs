using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;

namespace MentorBooking.Repository.Repositories;

public class MentorRepository : IMentorRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MentorRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> AddInformationMentorAsync(Mentor mentor)
    {
        try
        {
            var mentorResponse = _dbContext.Mentors.Where(x => x.MentorId == mentor.MentorId).ToList();
            if (mentorResponse.Count > 0)
            {
                _dbContext.RemoveRange(mentorResponse);
                await _dbContext.Mentors.AddAsync(mentor);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            await _dbContext.Mentors.AddAsync(mentor);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}