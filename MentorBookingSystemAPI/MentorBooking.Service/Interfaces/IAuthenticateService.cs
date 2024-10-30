using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.Interfaces
{
    public interface IAuthenticateService
    {
        Task<RegisterModelResponse> RegisterUserAsync(RegisterModelRequest registerModel);
        Task<SettingRoleModelResponse> SettingRoleAsync(SettingRoleModelRequest settingRoleModel);
        Task<LoginModelResponse> Login(LoginModelRequest loginModel);
        Task<LogoutModelResponse> Logout(LogoutModelRequest logoutModel);
        Task<RefreshTokenModelResponse> RefreshToken(RefreshTokenModelRequest refreshTokenModel);
        Task<string?> GetCofirmTokenAsync(ConfirmationEmailModelRequest confirmationEmailModel);
        Task<string> GenerateBodyMessageForConfirmationEmailAsync(
            ConfirmationEmailModelRequest confirmationEmailModel, string confirmLink);
        Task<bool> IsEmailConfirmedAsync(LinkEmailConfirmModelRequest linkEmailConfirm);
    }
}
