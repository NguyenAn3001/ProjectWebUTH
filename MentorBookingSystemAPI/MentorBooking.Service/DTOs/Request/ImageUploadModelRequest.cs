using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MentorBooking.Service.DTOs.Request;

public class ImageUploadModelRequest
{
    [Required(ErrorMessage = "User Id is required.")]
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Image is required.")]
    public IFormFile Image { get; set; }
}