using MentorBooking.Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Interfaces
{
    public interface IWorkSchedulesView
    {
        List<ApiResponse> MentorWorkScheulesViews(Guid MentorId);
        Task<List<ApiResponse>> StudentWorkScheulesViews(Guid StudentId);
        Task<List<ApiResponse>> SchedulesForBooking(Guid MentorId);
    }
}
