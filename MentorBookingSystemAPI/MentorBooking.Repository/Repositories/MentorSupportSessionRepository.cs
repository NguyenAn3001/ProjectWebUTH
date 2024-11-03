using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DeleteMentorSupportSessionAsync(Guid SessionId)
        {
            try
            {
                var deleteSession = await _dbContext.MentorSupportSessions.SingleOrDefaultAsync(temp => temp.SessionId == SessionId);
                if (deleteSession==null)
                {
                    return false;
                }
                _dbContext.MentorSupportSessions.Remove(deleteSession);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public List<MentorSupportSession>? GetAllMentorSupportSessionAsync(Guid MentorId)
        {
           var allMentorSupportSession= _dbContext.MentorSupportSessions.Where(temp=>temp.MentorId==MentorId).ToList();
            if (allMentorSupportSession.Count() == 0) return null;
            return allMentorSupportSession;
        }

        public async Task<MentorSupportSession?> GetMentorSupportSessionAsync(Guid SessionId)
        {
            var getSession = await _dbContext.MentorSupportSessions.SingleOrDefaultAsync(temp => temp.SessionId == SessionId);
            return getSession;
        }
    }
}
