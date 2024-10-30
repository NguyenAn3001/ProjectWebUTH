using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class ConfirmationEmailModelRequest
{
    [Required(ErrorMessage = "User Id for confirmation is required")]
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string? EmailGetConfirmToken { get; set; }
}