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
    public class SchedulesAvailableRepository : ISchedulesAvailableRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SchedulesAvailableRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddScheduleAvailableAsync(SchedulesAvailable schedulesAvailable)
        {
            var schedulesResponse = _dbContext.SchedulesAvailables.Where(temp=>temp.ScheduleAvailableId==schedulesAvailable.ScheduleAvailableId).ToList();
            try
            {
                if (schedulesResponse.Count() > 0)
                {
                    _dbContext.SchedulesAvailables.RemoveRange(schedulesResponse);
                    await _dbContext.SchedulesAvailables.AddAsync(schedulesAvailable);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                await _dbContext.SchedulesAvailables.AddAsync(schedulesAvailable);
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
