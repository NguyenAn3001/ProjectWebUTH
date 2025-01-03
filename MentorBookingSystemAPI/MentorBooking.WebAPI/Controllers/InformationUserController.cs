﻿using System.Security.Claims;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/info")]
public class InformationUserController : ControllerBase
{
    private readonly IUpdateInformationService _updateInformationService;

    public InformationUserController(IUpdateInformationService updateInformationService)
    {
        _updateInformationService = updateInformationService;
    }
    [AllowAnonymous]
    [HttpPost("mentor")]
    public async Task<IActionResult> MentorInformationUpdate(Guid mentorId, [FromBody] MentorInformationModelRequest mentorInformationModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse
            {
                Status = "Error",
                Message = "Model state is invalid"
            });
        }
        var updateInfoResponse = await _updateInformationService.UpdateMentorInformationAsync(mentorId, mentorInformationModel);
        return updateInfoResponse.Status switch
        {
            "Error" => BadRequest(updateInfoResponse),
            "ServerError" => StatusCode(500, updateInfoResponse),
            _ => Ok(updateInfoResponse)
        };
    }
    [AllowAnonymous]
    [HttpPost("student")]
    public async Task<IActionResult> StudentInformationUpdate(Guid studentId, [FromBody] StudentInformationModelRequest studentInformationModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse
            {
                Status = "Error",
                Message = "Model state is invalid"
            });
        }
        var updateInfoResponse = await _updateInformationService.UpdateStudentInformationAsync(studentId, studentInformationModel);
        return updateInfoResponse.Status switch
        {
            "Error" => BadRequest(updateInfoResponse),
            "ServerError" => StatusCode(500, updateInfoResponse),
            _ => Ok(updateInfoResponse)
        };
    }
}