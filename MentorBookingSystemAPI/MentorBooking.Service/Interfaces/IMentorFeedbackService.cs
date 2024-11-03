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
        Task<ApiResponse> AddStudentCommentAsync(StudentCommentRequest studentComment);
        Task<bool> UpdateMentorFeedbackAsync(MentorFeedback OldMentorFeedback, StudentCommentRequest studentComment);
        Task<ApiResponse> DeleteMentorFeedbackAsync(Guid MentorFeedbackId);
        List<ApiResponse> GetAllMentorFeedback(Guid MentorId);
        Task<ApiResponse> GetMentorFeedback(Guid MentorFeedbackId);
    }
}
