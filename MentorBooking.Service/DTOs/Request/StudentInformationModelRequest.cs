using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class StudentInformationModelRequest
{
    [Required(ErrorMessage = "Student's first name is required")]
    [Length(1, 100, ErrorMessage = "Student's first name must be between 1 and 100 characters")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Student's last name is required")]
    [Length(1, 100, ErrorMessage = "Student's last name must be between 1 and 100 characters")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "Student's phone number is required")]
    [Phone(ErrorMessage = "Student's phone number is invalid")]
    [StringLength(10, ErrorMessage = "Student's phone number must be 10 characters")]
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
}