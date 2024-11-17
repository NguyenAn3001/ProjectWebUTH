using System.Security.Claims;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/schedules")]
public class SchedulesController : ControllerBase
{
    private readonly ISchedulesMentor _schedulesMentor;

    public SchedulesController(ISchedulesMentor schedulesMentor)
    {
        _schedulesMentor = schedulesMentor;
    }
    
    [Authorize(Roles = "Mentor")]
    [HttpPost("add")]
    public async Task<IActionResult> AddSchedulesAvailable(
        [FromBody] List<SchedulesAvailableModelRequest> schedulesAvailableRequests)
    {
        var mentorId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
        }
        var addScheduleResponse = await _schedulesMentor.AddSchedulesAsync(Guid.Parse(mentorId), schedulesAvailableRequests);
        return addScheduleResponse.Status switch
        {
            "Error" => BadRequest(new
            {
                Status = addScheduleResponse.Status,
                Message = addScheduleResponse.Message
            }),
            "ServerError" => StatusCode(500, new
            {
                Status = addScheduleResponse.Status,
                Message = addScheduleResponse.Message
            }),
            _ => Ok(addScheduleResponse)
        };
    }
    [Authorize(Roles = "Mentor")]
    [HttpPut("{scheduleAvailableId}")]
    public async Task<IActionResult> UpdateMentorSchedule(Guid scheduleAvailableId,
        [FromBody] SchedulesAvailableModelRequest updateMentorScheduleRequest)
    {
        var mentorId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (!ModelState.IsValid)
            return BadRequest(new { message = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
        var updateScheduleResponse = await _schedulesMentor.UpdateScheduleAsync(Guid.Parse(mentorId), scheduleAvailableId, updateMentorScheduleRequest);
        return updateScheduleResponse.Status switch
        {
            "Error" => BadRequest(new
            {
                Status = updateScheduleResponse.Status,
                Message = updateScheduleResponse.Message
            }),
            _ => Ok(updateScheduleResponse)
        };
    }
    [Authorize(Roles = "Mentor")]
    [HttpDelete]
    public async Task<IActionResult> DeleteMentorSchedule([FromQuery]Guid scheduleAvailableId)
    {
        if (scheduleAvailableId == Guid.Empty)
            return BadRequest(new { message = "Available schedules is required" });
        var deleteScheduleResponse = await _schedulesMentor.DeleteScheduleAsync(scheduleAvailableId);
        return deleteScheduleResponse.Status switch
        {
            "Error" => BadRequest(new
            {
                Status = deleteScheduleResponse.Status,
                Message = deleteScheduleResponse.Message
            }),
            _ => Ok(new
            {
                Status = deleteScheduleResponse.Status,
                Message = deleteScheduleResponse.Message
            })
        };
    }
    [Authorize(Roles = "Mentor, Student")]
    [HttpGet("schedules-mentor")]
    public IActionResult GetSchedulesMentor([FromQuery]Guid mentorId)
    {
        if (mentorId == Guid.Empty)
            return BadRequest(new { message = "Invalid mentor" });
        var schedulesResponse = _schedulesMentor.GetSchedules(mentorId);
        return schedulesResponse.Status switch
        {
            "NotFound" => NotFound(schedulesResponse),
            _ => Ok(schedulesResponse)
        };
    }
}