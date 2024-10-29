namespace MentorBooking.Service.DTOs.Request;

public class LinkEmailConfirmModelRequest
{
    public string? UserId { get; set; }
    public string? Token { get; set; }
}