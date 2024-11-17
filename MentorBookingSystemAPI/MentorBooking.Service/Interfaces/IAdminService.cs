using MentorBooking.Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Interfaces
{
    public interface IAdminService
    {
        List<ApiResponse> GetAllSessions();
        Task<List<ApiResponse>?> GetAllPointTransactionsIncrease();
        Task<List<ApiResponse>?> GetAllPointTransactionsDecrease();
        Task<List<ApiResponse>> GetAllMentor();
        Task<List<ApiResponse>> GetAllStudent();
        Task<ApiResponse> DeletePointTransaction(Guid PointTransactionId);
        Task<ApiResponse> DeleteSession(Guid sessionId);
        Task<List<ApiResponse>> AllFeedback();
    }
}
