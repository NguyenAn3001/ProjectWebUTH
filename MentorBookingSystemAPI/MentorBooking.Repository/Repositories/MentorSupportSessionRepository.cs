﻿using MentorBooking.Repository.Data;
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
                if (sessionResponse.Count()>0)
                {
                    _dbContext.MentorSupportSessions.RemoveRange(sessionResponse);
                    await _dbContext.MentorSupportSessions.AddAsync(mentorSupportSession);
                    await _dbContext.SaveChangesAsync();
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

        public async Task<bool> CheckMentorSessionAsync(Guid SessionId)
        {
            var existSession = await _dbContext.MentorSupportSessions.SingleOrDefaultAsync(temp=>temp.SessionId==SessionId);
            if (existSession == null) return false;
            if(!existSession.SessionConfirm) return false;
            return true;
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

        public List<MentorSupportSession> GetALlSession()
        {
            var listSession = _dbContext.MentorSupportSessions.ToList();
            return listSession;
        }

        public async Task<MentorSupportSession?> GetMentorSupportSessionAsync(Guid SessionId)
        {
            var getSession = await _dbContext.MentorSupportSessions.Include(m => m.Group).SingleOrDefaultAsync(temp => temp.SessionId == SessionId);
            return getSession;
        }

        public async Task<MentorSupportSession?> GetMentorSupportSessionByGroupIdAsync(Guid GroupId)
        {
            var getSession = await _dbContext.MentorSupportSessions.SingleOrDefaultAsync(temp => temp.GroupId==GroupId);
            return getSession;
        }

        public async Task<bool> UpdateMentorSupportSessionAsync(MentorSupportSession mentorSupportSession)
        {
            try
            {
                var existMentorSupportSession = await _dbContext.MentorSupportSessions.SingleOrDefaultAsync(temp => temp.SessionId == mentorSupportSession.SessionId);
                if (existMentorSupportSession == null) return false;
                existMentorSupportSession.SessionConfirm = mentorSupportSession.SessionConfirm;
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
