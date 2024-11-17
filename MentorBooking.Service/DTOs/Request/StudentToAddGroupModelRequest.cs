using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class StudentToAddGroupModelRequest
{
    [Required(ErrorMessage = "Student ID is required")]
    public Guid StudentId { get; set; }
}