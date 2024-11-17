using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class ConfirmationEmailModelRequest
{
    [Required(ErrorMessage = "Email is required")]
    public string? EmailGetConfirmToken { get; set; }
}