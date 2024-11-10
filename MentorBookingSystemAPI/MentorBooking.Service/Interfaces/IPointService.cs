using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.Interfaces;

public interface IPointService
{
    Task<ApiResponse> AddPointAsync(Guid userId, PointChangedModelRequest pointAdd);
    Task<ApiResponse> CashPointAsync(Guid userId, PointCashModelRequest pointCash);
}