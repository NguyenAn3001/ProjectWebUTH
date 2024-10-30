using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class ImageGetModelRequest
{
    [Required(ErrorMessage = "User id is required for get image.")]
    public Guid UserId { get; set; }
}