using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;

namespace MentorBooking.Service.Services;

public class PointService : IPointService
{
    private readonly IUserPointRepository _userPointRepository;
    private readonly IUserRepository _userRepository;

    public PointService(IUserPointRepository userPointRepository, IUserRepository userRepository)
    {
        _userPointRepository = userPointRepository;
        _userRepository = userRepository;
    }
    public async Task<ApiResponse> AddPointAsync(Guid userId, PointChangedModelRequest pointAdd)
    {
        var pointExisting = await _userPointRepository.GetUserPoint(userId);
        var user = await _userRepository.FindByIdAsync(userId.ToString());
        var pointChange = pointExisting.PointsBalance + pointAdd.Points;
        if (!await _userPointRepository.SetUserPoint(userId, pointChange!.Value, $"{user?.FirstName} {user?.LastName} is add {pointAdd.Points} points."))
        {
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Add point failed, please try again later, or contact the administrator."
            };
        }
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Point added successfully.",
            Data = $"Point existing is {pointChange}"
        };
    }

    public async Task<ApiResponse> CashPointAsync(Guid userId, PointCashModelRequest pointCash)
    {
        var pointExisting = await _userPointRepository.GetUserPoint(userId);
        if (pointCash.Points >= pointExisting.PointsBalance)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "You cannot cash points higher than the current balance, please try again."
            };
        var user = await _userRepository.FindByIdAsync(userId.ToString());
        var pointChange = pointExisting.PointsBalance - pointCash.Points;
        if (!await _userPointRepository.SetUserPoint(userId, pointChange!.Value, $"{user?.FirstName} {user?.LastName} is cash out {pointCash.Points} points."))
        {
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Cash out failed, please try again later, or contact the administrator."
            };
        }
        return new ApiResponse()
        {
            Status = "Success",
            Message = $"Point cashed successfully, please wait for 15 minutes money will be sending to {pointCash.PaypalAddress}.",
            Data = $"Point existing is {pointChange}"
        };
    }
}