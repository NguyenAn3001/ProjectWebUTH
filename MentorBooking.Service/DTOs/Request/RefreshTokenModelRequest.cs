using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class RefreshTokenModelRequest
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string? RefreshToken { get; set; }
}