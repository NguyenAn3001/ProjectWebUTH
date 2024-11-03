using System.Security.Claims;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/[controller]")]
public class InformationUserController : ControllerBase
{
    private readonly IUpdateInformationService _updateInformationService;

    public InformationUserController(IUpdateInformationService updateInformationService)
    {
        _updateInformationService = updateInformationService;
    }
    [Authorize(Roles = "Admin, Mentor")]
    [HttpPost("mentor-info")]
    public async Task<IActionResult> MentorInformationUpdate([FromBody] MentorInformationModelRequest mentorInformationModel)
    {
        var mentorId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse
            {
                Status = "Error",
                Message = "Model state is invalid"
            });
        }
        var updateInfoResponse = await _updateInformationService.UpdateMentorInformationAsync(Guid.Parse(mentorId), mentorInformationModel);
        return updateInfoResponse.Status switch
        {
            "Error" => BadRequest(updateInfoResponse),
            "ServerError" => StatusCode(500, updateInfoResponse),
            _ => Ok(updateInfoResponse)
        };
    }
    [Authorize(Roles = "Admin, Student")]
    [HttpPost("student-info")]
    public async Task<IActionResult> StudentInformationUpdate([FromBody] StudentInformationModelRequest studentInformationModel)
    {
        var studentId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse
            {
                Status = "Error",
                Message = "Model state is invalid"
            });
        }
        var updateInfoResponse = await _updateInformationService.UpdateStudentInformationAsync(Guid.Parse(studentId), studentInformationModel);
        return updateInfoResponse.Status switch
        {
            "Error" => BadRequest(updateInfoResponse),
            "ServerError" => StatusCode(500, updateInfoResponse),
            _ => Ok(updateInfoResponse)
        };
    }
}