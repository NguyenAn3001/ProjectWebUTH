using System.Security.Claims;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly IGroupOfStudentService _groupOfStudentService;

    public GroupController(IGroupOfStudentService groupOfStudentService)
    {
        _groupOfStudentService = groupOfStudentService;
    }
    [Authorize(Roles = "Student")]
    [HttpPost("create-new-group")]
    public async Task<IActionResult> CreateNewGroup([FromBody] CreateGroupModelRequest createGroupModelRequest)
    {
        var studentId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
        var createGroupResponse = await _groupOfStudentService.CreateGroupAsync(Guid.Parse(studentId!), createGroupModelRequest);
        return createGroupResponse.Status switch
        {
            "Error" => BadRequest(new { status = createGroupResponse.Status, message = createGroupResponse.Message }),
            _ => Ok(new { status = createGroupResponse.Status, message = createGroupResponse.Message })
        };
    }
    [Authorize(Roles = "Student")]
    [HttpPost("add-member-group/{groupId}")]
    public async Task<IActionResult> AddMemberToGroup(int groupId, List<StudentToAddGroupModelRequest> students)
    {
        var studentId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
        var addStudentToGroupResponse = await _groupOfStudentService.AddStudentToGroupAsync(Guid.Parse(studentId!), groupId, students);
        return addStudentToGroupResponse.Status switch
        {
            "Error" => BadRequest(new { status = addStudentToGroupResponse.Status, message = addStudentToGroupResponse.Message }),
            "Unauthorized" => Unauthorized(new
                { status = addStudentToGroupResponse.Status, message = addStudentToGroupResponse.Message }),
            _ => Ok(addStudentToGroupResponse)
        };
    }
}