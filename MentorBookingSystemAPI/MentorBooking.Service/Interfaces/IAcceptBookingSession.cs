﻿using MentorBooking.Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Interfaces
{
    public interface IAcceptBookingSession
    {
        List<ApiResponse> GetAllSessionUnAccept(Guid MentorId);
        Task<ApiResponse> AcceptSession(Guid SessionId,bool acceptSession);
    }
}
