using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Repository.Repositories;

public class SchedulesAvailableRepository : ISchedulesAvailableRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SchedulesAvailableRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> AddSchedulesAvailableAsync(List<SchedulesAvailable> schedulesAvailable)
    {
        try
        {
            foreach (var schedule in schedulesAvailable)
            {
                var scheduleExisting = await _dbContext.SchedulesAvailable.SingleOrDefaultAsync(x => x.MentorId == schedule.MentorId && x.FreeDay == schedule.FreeDay && x.StartTime == schedule.StartTime && x.EndTime == schedule.EndTime);
                if (scheduleExisting == null)
                {
                    await _dbContext.SchedulesAvailable.AddAsync(schedule);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public List<SchedulesAvailable>? GetAllSchedulesAvailable(Guid mentorId)
    {
        var schedulesAvailable =  _dbContext.SchedulesAvailable.Where(x => x.MentorId == mentorId).ToList();
        if (schedulesAvailable.Count == 0)
            return null;
        return schedulesAvailable;
    }

    public async Task<bool> UpdateSchedulesAvailableAsync(Guid scheduleAvailableId, SchedulesAvailable schedulesAvailable)
    {
        try
        {
            var scheduleToUpdating = await _dbContext.SchedulesAvailable.SingleOrDefaultAsync(x => x.ScheduleAvailableId == scheduleAvailableId);
            if (scheduleToUpdating == null)
                return false;
            scheduleToUpdating.FreeDay = schedulesAvailable.FreeDay;
            scheduleToUpdating.StartTime = schedulesAvailable.StartTime;
            scheduleToUpdating.EndTime = schedulesAvailable.EndTime;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> DeleteSchedulesAvailableAsync(Guid schedulesAvailableId)
    {
        try
        {
            var scheduleToDelete = await _dbContext.SchedulesAvailable.SingleOrDefaultAsync(x => x.ScheduleAvailableId == schedulesAvailableId);
            if (scheduleToDelete == null)
                return false;
            _dbContext.SchedulesAvailable.Remove(scheduleToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<SchedulesAvailable?> GetSchedulesAvailableAsync(Guid schedulesAvailableId)
    {
        var schedule = await _dbContext.SchedulesAvailable.SingleOrDefaultAsync(x => x.ScheduleAvailableId == schedulesAvailableId);
        return schedule;
    }
}
