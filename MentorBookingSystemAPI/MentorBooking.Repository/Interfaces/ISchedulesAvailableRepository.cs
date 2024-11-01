using MentorBooking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Interfaces
{
    public interface ISchedulesAvailableRepository
    {
        Task<bool> AddScheduleAvailableAsync(SchedulesAvailable schedulesAvailable);
    }
}
