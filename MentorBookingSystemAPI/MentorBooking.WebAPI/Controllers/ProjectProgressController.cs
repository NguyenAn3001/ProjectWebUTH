using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers;
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/progress")]
public class ProjectProgressController : ControllerBase
{
    private readonly IProjectProgressService _projectProgressService;

    public ProjectProgressController(IProjectProgressService projectProgressService)
    {
        _projectProgressService = projectProgressService;
    }
    [Authorize(Roles = "Mentor")]
    [HttpPost("update-progress")]
    public async Task<IActionResult> UpdateProgress(CreateProjectProgressModelRequest createProjectProgressModelRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "Error", message = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
        }
        var createProgressResponse = await _projectProgressService.CreateProjectProgressAsync(createProjectProgressModelRequest);
        return createProgressResponse.Status switch
        {
            "Error" => StatusCode(500, new
            {
                status = createProgressResponse.Status,
                message = createProgressResponse.Message
            }),
            _ => Ok(createProgressResponse)
        };
    }
    [Authorize(Roles = "Mentor")]
    [HttpPut("update-progress")]
    public async Task<IActionResult> UpdateProgress(UpdateProjectProgressModelRequest updateProjectProgressModelRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "Error", message = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
        }
        var updateProgressResponse = await _projectProgressService.UpdateProjectProgressAsync(updateProjectProgressModelRequest);
        return updateProgressResponse.Status switch
        {
            "NotFound" => NotFound(new
            {
                status = updateProgressResponse.Status,
                message = updateProgressResponse.Message
            }),
            "Error" => StatusCode(500, new
            {
                status = updateProgressResponse.Status,
                message = updateProgressResponse.Message
            }),
            _ => Ok(updateProgressResponse)
        };
    }
    [Authorize(Roles = "Mentor")]
    [HttpDelete("delete-progress")]
    public async Task<IActionResult> DeleteProgress([FromQuery]Guid projectProgressId)
    {
        if (projectProgressId.GetType() != typeof(Guid))
            return BadRequest(new
            {
                status = "Error",
                message = "Project progress id is not a Guid"
            });
        if (Guid.Empty == projectProgressId)
            return BadRequest(new {status = "Error", message = "Project progress id is null"});
        var deleteProgressResponse = await _projectProgressService.DeleteProjectProgressAsync(projectProgressId);
        return deleteProgressResponse.Status switch
        {
            "NotFound" => NotFound(new
            {
                status = deleteProgressResponse.Status,
                message = deleteProgressResponse.Message
            }),
            "Error" => StatusCode(500, new
            {
                status = deleteProgressResponse.Status,
                message = deleteProgressResponse.Message
            }),
            _ => Ok(deleteProgressResponse)
        };
    }
    [Authorize]
    [HttpGet("{projectProgressId}")]
    public async Task<IActionResult> GetProgress(Guid projectProgressId)
    {
        if (projectProgressId.GetType() != typeof(Guid))
            return BadRequest(new {status = "Error", message = "Project progress id is not a Guid"});
        if (Guid.Empty == projectProgressId)
            return BadRequest(new {status = "Error", message = "Project progress id is null"});
        var getProgressResponse = await _projectProgressService.GetProjectProgressAsync(projectProgressId);
        return getProgressResponse.Status switch
        {
            "NotFound" => NotFound(new
            {
                status = getProgressResponse.Status,
                message = getProgressResponse.Message
            }),
            _ => Ok(getProgressResponse)
        };
    }
    [Authorize]
    [HttpGet("session/{sessionId}")]
    public async Task<IActionResult> GetAllProgress(Guid sessionId)
    {
        if (sessionId.GetType() != typeof(Guid))
            return BadRequest(new {status = "Error", message = "Session id is not a Guid"});
        if (Guid.Empty == sessionId)
            return BadRequest(new {status = "Error", message = "Session id is null"});
        var getAllProgressResponse = await _projectProgressService.GetAllProjectProgressAsync(sessionId);
        return getAllProgressResponse.Status switch
        {
            "NotFound" => NotFound(new
            {
                status = getAllProgressResponse.Status,
                message = getAllProgressResponse.Message
            }),
            _ => Ok(getAllProgressResponse)
        };
    }
}