using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class SchedulesAvailableModelRequest
{
    [Required(ErrorMessage = "Free Day is required")]
    public DateOnly FreeDay { get; set; }
    [Required(ErrorMessage = "Start time is required")]
    public string StartTime { get; set; }
    [Required(ErrorMessage = "End time is required")]
    public string EndTime { get; set; }
}