using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class EmailMessageModelRequest
{
    [Required(ErrorMessage = "Email address is required")]
    public string? ToMailAddress { get; set; }
    [Required(ErrorMessage = "Subject is required")]
    public string? Subject { get; set; }
    [Required(ErrorMessage = "Body is required")]
    public string? Body { get; set; }
}