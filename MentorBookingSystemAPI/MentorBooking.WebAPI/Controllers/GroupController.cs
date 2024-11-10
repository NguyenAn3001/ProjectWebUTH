using System.Security.Claims;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/group")]
public class GroupController : ControllerBase
{
    private readonly IGroupOfStudentService _groupOfStudentService;

    public GroupController(IGroupOfStudentService groupOfStudentService)
    {
        _groupOfStudentService = groupOfStudentService;
    }
    [Authorize(Roles = "Student")]
    [HttpPost("new-group")]
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
    [HttpPost("add-member/{groupId}")]
    public async Task<IActionResult> AddMemberToGroup(Guid groupId, List<StudentToAddGroupModelRequest> students)
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
    [Authorize(Roles = "Student")]
    [HttpGet]
    public async Task<IActionResult> GetAllGroups()
    {
        var studentId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var getGroupsResponse = await _groupOfStudentService.GetAllGroupsAsync(Guid.Parse(studentId!));
        return getGroupsResponse.Status switch
        {
            "Error" => BadRequest(new { status = getGroupsResponse.Status, message = getGroupsResponse.Message }),
            _ => Ok(getGroupsResponse)
        };
    }
    [Authorize(Roles = "Student")]
    [HttpGet("your-groups")]
    public async Task<IActionResult> GetYourCreatedGroups()
    {
        var studentId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var getGroupsResponse = await _groupOfStudentService.GetYourCreatedGroupAsync(Guid.Parse(studentId!));
        return getGroupsResponse.Status switch
        {
            "Error" => BadRequest(new { status = getGroupsResponse.Status, message = getGroupsResponse.Message }),
            "NotFound" => NotFound(new {status = getGroupsResponse.Status, message = getGroupsResponse.Message }),
            _ => Ok(getGroupsResponse)
        };
    }
    [Authorize(Roles = "Student")]
    [HttpDelete("your-groups/{groupId}")]
    public async Task<IActionResult> DeleteGroup(Guid groupId)
    {
        var studentId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var deleteGroupResponse = await _groupOfStudentService.DeleteGroupAsync(Guid.Parse(studentId!), groupId);
        return deleteGroupResponse.Status switch
        {
            "Error" => BadRequest(new { status = deleteGroupResponse.Status, message = deleteGroupResponse.Message }),
            _ => Ok(new { status = deleteGroupResponse.Status, message = deleteGroupResponse.Message })
        };
    }
    [Authorize(Roles = "Student")]
    [HttpPut("your-groups/{groupId}")]
    public async Task<IActionResult> UpdateGroup(Guid groupId, [FromBody] CreateGroupModelRequest updateGroupModelRequest)
    {
        var studentId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var updateGroupResponse = await _groupOfStudentService.UpdateGroupAsync(Guid.Parse(studentId!), groupId, updateGroupModelRequest);
        return updateGroupResponse.Status switch
        {
            "Error" => BadRequest(new { status = updateGroupResponse.Status, message = updateGroupResponse.Message }),
            "NotFound" => NotFound(new { status = updateGroupResponse.Status, message = updateGroupResponse.Message }),
            _ => Ok(new { status = updateGroupResponse.Status, message = updateGroupResponse.Message })
        };
    }
    [Authorize(Roles = "Student")]
    [HttpDelete("your-groups")]
    public async Task<IActionResult> DeleteMemberGroup([FromQuery]Guid groupId,[FromQuery]Guid memberId)
    {
        var studentId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var deleteMemberResponse = await _groupOfStudentService.DeleteStudentMemberAsync(Guid.Parse(studentId!), groupId, memberId);
        return deleteMemberResponse.Status switch
        {
            "Error" => BadRequest(new { status = deleteMemberResponse.Status, message = deleteMemberResponse.Message }),
            "Unauthorized" => Unauthorized(new { status = deleteMemberResponse.Status, message = deleteMemberResponse.Message }),
            _ => Ok(new { status = deleteMemberResponse.Status, message = deleteMemberResponse.Message })
        };
    }
}