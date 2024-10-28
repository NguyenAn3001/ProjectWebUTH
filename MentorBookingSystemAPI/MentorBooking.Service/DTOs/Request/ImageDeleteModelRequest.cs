using System.ComponentModel.DataAnnotations;

namespace MentorBooking.Service.DTOs.Request;

public class ImageDeleteModelRequest
{
    [Required(ErrorMessage = "User id is required for delete image.")]
    public Guid UserId { get; set; }
}