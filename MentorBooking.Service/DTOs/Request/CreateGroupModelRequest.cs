using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class CreateGroupModelRequest
{
    [Required(ErrorMessage = "Group name is required")]
    [MinLength(3, ErrorMessage = "Group name must be at least 3 characters long")]
    [MaxLength(100, ErrorMessage = "Group name cannot exceed 100 characters")]
    public string? GroupName { get; set; }
    [Required(ErrorMessage = "Description topic is required")]
    [MinLength(3, ErrorMessage = "Topic name must be at least 3 characters long")]
    [MaxLength(100, ErrorMessage = "Topic name cannot exceed 100 characters")]
    public string? Topic { get; set; }
}