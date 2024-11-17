using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class MentorInformationModelRequest
{
    [Required(ErrorMessage = "Mentor's first name is required")]
    [Length(1, 100, ErrorMessage = "Mentor's first name must be between 1 and 100 characters")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Mentor's last name is required")]
    [Length(1, 100, ErrorMessage = "Mentor's last name must be between 1 and 100 characters")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "Mentor's phone number is required")]
    [Phone(ErrorMessage = "Mentor's phone number is invalid")]
    [StringLength(10, ErrorMessage = "Mentor's phone number must be 10 characters")]
    public string? Phone { get; set; }
    [Required(ErrorMessage = "Mentor's experience number is required")]
    public byte? ExperienceYears { get; set; }
    [MaxLength(1000, ErrorMessage = "Mentor's experience number has maximum length is 1000 characters")]
    public string? MentorDescription { get; set; }
    [Required(ErrorMessage = "Mentor's skills is required")]
    public List<string>? Skills { get; set; }
    public DateTime CreatedAt { get; set; }
}