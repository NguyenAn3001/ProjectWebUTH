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
    public class MentorWorkScheduleRepository : IMentorWorkScheduleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MentorWorkScheduleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddMentorWordScheduleAsync(MentorWorkSchedule mentorWorkSchedule)
        {
            try
            {
                var workSchedule = _dbContext.MentorWorkSchedules.Where(temp =>temp.ScheduleAvailableId==mentorWorkSchedule.ScheduleAvailableId).ToList();
                if(workSchedule.Count>0)
                {
                    _dbContext.MentorWorkSchedules.RemoveRange(workSchedule);
                    await _dbContext.MentorWorkSchedules.AddAsync(mentorWorkSchedule);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                await _dbContext.MentorWorkSchedules.AddAsync(mentorWorkSchedule);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> CheckWorkScheduleAsync(Guid mentorWorkScheduleId)
        {
            var check= await _dbContext.MentorWorkSchedules.SingleOrDefaultAsync(temp=>temp.ScheduleAvailableId==mentorWorkScheduleId);
            if(check != null)
            {
                 return true;
            }
            return false;
        }

        public async Task<bool> DeleteMentorWorkScheduleAsync(Guid SessionId)
        {
            try
            {
                var deleteWorkSchedule = _dbContext.MentorWorkSchedules.Where(temp => temp.SessionId == SessionId).ToList();
                if (deleteWorkSchedule == null) 
                {
                    return false;
                }
                _dbContext.MentorWorkSchedules.RemoveRange(deleteWorkSchedule);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public List<MentorWorkSchedule> GetMentorWorkSchedule(Guid SessionId)
        {
            var getWorkSchedule = _dbContext.MentorWorkSchedules.Where(temp => temp.SessionId==SessionId).ToList();
            return getWorkSchedule;
        }

        public MentorWorkSchedule? GetMentorWorkSchedulesView(Guid SchedulesId)
        {
            var getWorkSchedule = _dbContext.MentorWorkSchedules.SingleOrDefault(temp => temp.ScheduleAvailableId==SchedulesId);
            if (getWorkSchedule == null) return null;
            return getWorkSchedule;
        }

        public async Task<bool> UpdateMentorWorkSchedule(MentorWorkSchedule mentorWorkSchedule)
        {
            try
            {
                var existMentorSupportSession = await _dbContext.MentorWorkSchedules.SingleOrDefaultAsync(temp => temp.ScheduleId == mentorWorkSchedule.ScheduleId);
                if (existMentorSupportSession == null) return false;
                existMentorSupportSession.UnavailableDate = mentorWorkSchedule.UnavailableDate;
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
