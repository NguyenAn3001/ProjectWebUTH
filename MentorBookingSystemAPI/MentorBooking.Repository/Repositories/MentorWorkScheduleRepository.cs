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
                var workSchedule = _dbContext.MentorWorkSchedules.Where(temp => temp.ScheduleId == mentorWorkSchedule.ScheduleId).ToList();
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

        public async Task<bool> CheckWordScheduleAsync(Guid mentorWorkScheduleId)
        {
            var check=_dbContext.MentorWorkSchedules.FirstOrDefault(temp=>temp.ScheduleAvailableId==mentorWorkScheduleId);
            if(check != null)
            {
                if (check.UnavailableDate) return true;
            }
            return false;
        }
    }
}
