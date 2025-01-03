﻿using MentorBooking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Interfaces;

public interface ISchedulesAvailableRepository
{
    Task<bool> AddSchedulesAvailableAsync(List<SchedulesAvailables> schedulesAvailable);
    List<SchedulesAvailables>? GetAllSchedulesAvailable(Guid mentorId);
    Task<bool> UpdateSchedulesAvailableAsync(Guid scheduleAvailableId, SchedulesAvailables schedulesAvailable);
    Task<bool> DeleteSchedulesAvailableAsync(Guid schedulesAvailableId);
    Task<SchedulesAvailables?> GetSchedulesAvailableAsync(Guid schedulesAvailableId);
}

