using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers;
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/[controller]")]
public class AvatarController : ControllerBase
{
    private readonly IImageUploadService _imageUploadService;

    public AvatarController(IImageUploadService imageUploadService)
    {
        _imageUploadService = imageUploadService;
    }
    [Authorize]
    [HttpPost("upload-avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] ImageUploadModelRequest request)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
        if (!ModelState.IsValid)
        {
            return BadRequest(new { errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
        }
        var uploadResponse = await _imageUploadService.UploadImageAsync(baseUrl, request);
        return uploadResponse.Status switch
        {
            "Error" => BadRequest(uploadResponse),
            "ServerError" => StatusCode(500, uploadResponse),
            _ => Ok(uploadResponse)
        };
    }
    [Authorize]
    [HttpGet("get-avatar")]
    public async Task<IActionResult> GetAvatar([FromQuery] Guid userId)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
        if (string.IsNullOrEmpty(userId.ToString()))
        {
            return BadRequest(new ImageGetModelResponse()
            {
                Status = "Error",
                Message = "Please provide user id."
            });
        }
        ImageGetModelRequest request = new ImageGetModelRequest()
        {
            UserId = userId
        };
        var imageGetModelResponse = await _imageUploadService.GetImageByIdAsync(request);
        return imageGetModelResponse.Status switch
        {
            "Error" => BadRequest(imageGetModelResponse),
            _ => Ok(new ImageGetModelResponse()
            {
                Status = imageGetModelResponse.Status,
                Message = imageGetModelResponse.Message,
                ImageUrl = baseUrl + imageGetModelResponse.ImageUrl
            })
        };
    }
    [Authorize]
    [HttpDelete("delete-avatar")]
    public async Task<IActionResult> DeleteAvatar([FromQuery] Guid userId)
    {
        if (string.IsNullOrEmpty(userId.ToString()))
        {
            return BadRequest(new ImageGetModelResponse()
            {
                Status = "Error",
                Message = "Please provide user id."
            });
        }
        ImageDeleteModelRequest request = new ImageDeleteModelRequest(){UserId = userId};
        var imageDeleteModelResponse = await _imageUploadService.DeleteImageAsync(request);
        return imageDeleteModelResponse.Status switch
        {
            "Error" => BadRequest(imageDeleteModelResponse),
            _ => Ok(imageDeleteModelResponse)
        };
    }
}