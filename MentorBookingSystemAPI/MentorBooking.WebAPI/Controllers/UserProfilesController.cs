using MentorBooking.Repository.Entities;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MentorBooking.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly IStudentProfilesServices _studentProfilesServices;
        private readonly IMentorProfilesService _mentorProfilesService;
        public UserProfilesController(IStudentProfilesServices studentProfilesServices, IMentorProfilesService mentorProfilesService)
        {
            _studentProfilesServices = studentProfilesServices;
            _mentorProfilesService = mentorProfilesService;
        }
        [Authorize]
        [HttpGet("student-profiles")]
        public async Task<IActionResult> StudentProfiles (Guid StudentId)
        {
            if (StudentId == Guid.Empty)
                return BadRequest(new { message = "SessionId is required" });
            var studentProfiles =await _studentProfilesServices.StudentProfiles(StudentId);
            return studentProfiles.Status switch
            {
                "Error" => BadRequest(new { status = studentProfiles.Status, message = studentProfiles.Message}),
                _ => Ok(new { status = studentProfiles.Status, message = studentProfiles.Message, Data = studentProfiles.Data })
            };
        }
        [Authorize]
        [HttpGet("mentor-profiles")]
        public async Task<IActionResult> MentorProfiles(Guid mentorId)
        {
            if (mentorId == Guid.Empty)
                return BadRequest(new { message = "SessionId is required" });
            var mentorProfiles = await _mentorProfilesService.MentorProfiles(mentorId);
            return mentorProfiles.Status switch
            {
                "Error" => BadRequest(new { status = mentorProfiles.Status, message = mentorProfiles.Message }),
                _ => Ok(new { status = mentorProfiles.Status, message = mentorProfiles.Message, Data = mentorProfiles.Data })
            };
        }
        [Authorize(Roles = "Student")]
        [HttpGet("my-student-profiles")]
        public async Task<IActionResult> MyStudentProfiles()
        {
            var StudentId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var studentProfiles = await _studentProfilesServices.StudentProfiles(Guid.Parse(StudentId!));
            return studentProfiles.Status switch
            {
                "Error" => BadRequest(new { status = studentProfiles.Status, message = studentProfiles.Message }),
                _ => Ok(new { status = studentProfiles.Status, message = studentProfiles.Message, Data = studentProfiles.Data })
            };
        }
        [Authorize(Roles = "Mentor")]
        [HttpGet("my-mentor-profiles")]
        public async Task<IActionResult> MyMentorProfiles()
        {
            var MentorId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var mentorProfiles = await _mentorProfilesService.MentorProfiles(Guid.Parse(MentorId!));
            return mentorProfiles.Status switch
            {
                "Error" => BadRequest(new { status = mentorProfiles.Status, message = mentorProfiles.Message }),
                _ => Ok(new { status = mentorProfiles.Status, message = mentorProfiles.Message, Data = mentorProfiles.Data })
            };
        }
    }
}
