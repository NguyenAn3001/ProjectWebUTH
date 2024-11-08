using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
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
        Task<ApiResponse> BookingMentor(MentorSupportSessionRequest request,string userId);
        Task<ApiResponse> GetMentorSupportSessionAsync(Guid SessionId);
        Task<ApiResponse> DeleteMentorSupportSessionAsync(Guid SessionId);
        List<ApiResponse>? GetAllMentorSupportSessionAsync(Guid MentorId);
    }
}
