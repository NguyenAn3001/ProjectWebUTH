using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class CreateProjectProgressModelRequest
{
    [Required(ErrorMessage = "Session id is required")]
    public Guid? SessionId { get; set; }
    [Required(ErrorMessage = "Description is required")]
    [MaxLength(10000, ErrorMessage = "Description length is too long")]
    [MinLength(3, ErrorMessage = "Description length is too short")]
    public string? Description { get; set; }
}