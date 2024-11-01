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
    public interface IMentorFeedbackService
    {
        Task<ApiResponse> AddMentorFeedbackAsync(Users user,Guid mentorId,Guid sessionId ,StudentCommentRequest studentComment);
        Task<bool> UpdateMentorFeedbackAsync(MentorFeedback OldMentorFeedback, StudentCommentRequest studentComment);
    }
}
