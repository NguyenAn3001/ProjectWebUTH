using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DeleteMentorFeedbackAsync(Guid FeedbackId)
        {
            try
            {
                var deleteMentorFeedback = await _dbContext.MentorFeedbacks.SingleOrDefaultAsync(temp => temp.FeedbackId==FeedbackId);
                if(deleteMentorFeedback == null)
                {
                    return false;
                }
                _dbContext.MentorFeedbacks.Remove(deleteMentorFeedback);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public List<MentorFeedback>? GetAllMentorFeedbacksAsync(Guid MentorId)
        {
            var allMentorFeedback = _dbContext.MentorFeedbacks.Where(temp=>temp.MentorId == MentorId).ToList();
            if (allMentorFeedback.Count() == 0) return null;
            return allMentorFeedback;
        }

        public async Task<MentorFeedback?> GetMentorFeedbackAsync(Guid FeedbackId)
        {
            var getMentorFeedback = await _dbContext.MentorFeedbacks.SingleOrDefaultAsync(temp => temp.FeedbackId==FeedbackId);
            return getMentorFeedback;
        }

        public async Task<bool> UpdateMentorFeedbackAsync(MentorFeedback mentorFeedback)
        {
            try
            {
                var feedbackResponse = _dbContext.MentorFeedbacks.Where(x => x.SessionId == mentorFeedback.SessionId && x.StudentId == mentorFeedback.StudentId).ToList();
                if (feedbackResponse.Count > 0)
                {
                    _dbContext.RemoveRange(feedbackResponse);
                    await _dbContext.MentorFeedbacks.AddAsync(mentorFeedback);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
