using MentorBooking.Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Interfaces
{
    public interface IStudentProfilesServices
    {
        Task<ApiResponse> StudentProfiles(Guid StudentId);
    }
}
