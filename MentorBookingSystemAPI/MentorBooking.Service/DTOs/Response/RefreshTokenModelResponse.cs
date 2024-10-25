namespace MentorBooking.Service.DTOs.Response;

public class RefreshTokenModelResponse
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }   
}