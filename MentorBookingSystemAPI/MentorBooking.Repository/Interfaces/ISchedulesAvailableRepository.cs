using MentorBooking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Interfaces;

public interface ISchedulesAvailableRepository
{
    Task<bool> AddSchedulesAvailableAsync(List<SchedulesAvailable> schedulesAvailable);
    List<SchedulesAvailable>? GetAllSchedulesAvailable(Guid mentorId);
    Task<bool> UpdateSchedulesAvailableAsync(Guid scheduleAvailableId, SchedulesAvailable schedulesAvailable);
    Task<bool> DeleteSchedulesAvailableAsync(Guid schedulesAvailableId);
    Task<SchedulesAvailable?> GetSchedulesAvailableAsync(Guid schedulesAvailableId);
}

