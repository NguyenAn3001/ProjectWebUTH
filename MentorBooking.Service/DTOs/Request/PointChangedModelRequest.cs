using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class PointChangedModelRequest
{
    [Required(ErrorMessage = "Please enter the point...")]
    [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Please enter a valid number")]
    public int Points { get; set; }
}