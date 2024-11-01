﻿using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Interfaces
{
    public interface IBookingMentorService
    {
        Task<ApiResponse> BookingMentor(MentorSupportSessionRequest request);
    }
}