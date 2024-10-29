using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Repositories
{
    public class MentorFeedbackRepository : IMentorFeedbackRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MentorFeedbackRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddMentorFeedbackAsync(MentorFeedback mentorFeedback)
        {
            try
            {
                var feedbackResponse = _dbContext.MentorFeedbacks.Where(x => x.MentorId==mentorFeedback.MentorId && x.StudentId==mentorFeedback.StudentId).ToList();
                if(feedbackResponse.Count > 0)
                {
                    _dbContext.RemoveRange(feedbackResponse);
                    await _dbContext.MentorFeedbacks.AddAsync(mentorFeedback);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                await _dbContext.MentorFeedbacks.AddAsync(mentorFeedback);
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
}
