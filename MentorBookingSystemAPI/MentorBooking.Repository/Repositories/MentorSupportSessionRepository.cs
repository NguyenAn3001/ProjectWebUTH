using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Repositories
{
    public class MentorSupportSessionRepository : IMentorSupportSessionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MentorSupportSessionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddMentorSupportSessionAsync(MentorSupportSession mentorSupportSession)
        {
            try
            {
                var sessionResponse = _dbContext.MentorSupportSessions.Where(temp => temp.SessionId == mentorSupportSession.SessionId).ToList();
                if(sessionResponse.Count>0)
                {
                    _dbContext.RemoveRange(sessionResponse);
                    await _dbContext.MentorSupportSessions.AddAsync(mentorSupportSession);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                await _dbContext.MentorSupportSessions.AddAsync(mentorSupportSession);
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
