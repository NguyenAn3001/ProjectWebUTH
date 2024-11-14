using System.Text;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;

namespace MentorBooking.Service.Services;

public class PointService : IPointService
{
    private readonly IUserPointRepository _userPointRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISenderEmail _senderEmail;

    public PointService(IUserPointRepository userPointRepository, IUserRepository userRepository, ISenderEmail senderEmail)
    {
        _userPointRepository = userPointRepository;
        _userRepository = userRepository;
        _senderEmail = senderEmail;
    }

    public async Task<ApiResponse> ExistingPointAsync(Guid userId)
    {
        var points = await _userPointRepository.GetUserPoint(userId);
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Getting point success.",
            Data = new
            {
                Points = points.PointsBalance
            }
        };
        
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
        await _senderEmail.SendEmailAsync(user?.Email!, "Your Point Has Been Changed! - Details Inside", BuildPointChangeEmailBody(user?.UserName!, "add", pointAdd.Points, pointChange.Value, $"Point added successfully."), isBodyHtml: true);
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
        await _senderEmail.SendEmailAsync(user?.Email!, "Your Point Has Been Changed! - Details Inside", BuildPointChangeEmailBody(user?.UserName!, "cash", pointCash.Points, pointChange.Value, $"Please wait for 15 minutes money will be sending to {pointCash.PaypalAddress}."), isBodyHtml: true);
        return new ApiResponse()
        {
            Status = "Success",
            Message = $"Point cashed successfully, please wait for 15 minutes money will be sending to {pointCash.PaypalAddress}.",
            Data = $"Point existing is {pointChange}"
        };
    }
    private string BuildPointChangeEmailBody(string userName, string action, int pointsChanged, int newPointsBalance, string description)
    {
        var sb = new StringBuilder();
        sb.Append("<div style='font-family: Arial, sans-serif; margin: 20px;'>");
    
        // Email header
        sb.Append("<h2 style='color: #4CAF50;'>Hey, " + userName + "!</h2>");
    
        // Point change message based on action
        if (action == "add")
        {
            sb.Append($"<p style='color: #333;'>You have successfully added {pointsChanged} points to your account.</p>");
        }
        else if (action == "cash")
        {
            sb.Append($"<p style='color: #333;'>You have successfully cashed out {pointsChanged} points from your account, please wait a moment for admin to approve withdrawal.</p>");
        }

        // Display the new balance and description of the action
        sb.Append("<p>Your new points balance is: <strong>" + newPointsBalance + " points</strong>.</p>");
        sb.Append($"<p><i>{description}</i></p>");

        sb.Append("<hr style='border-top: 1px solid #ddd;' />");

        // Closing statement
        sb.Append("<p style='color: gray; font-size: 12px;'>");
        sb.Append("If you did not request this change, please contact support immediately.</p>");

        sb.Append("<p style='color: gray; font-size: 12px;'>");
        sb.Append("© 2024 Mentor Booking System. All rights reserved.</p>");

        sb.Append("</div>");

        return sb.ToString();
    }

}