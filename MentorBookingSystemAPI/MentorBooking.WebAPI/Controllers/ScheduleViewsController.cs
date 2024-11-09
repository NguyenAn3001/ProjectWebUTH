using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MentorBooking.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleViewsController : ControllerBase
    {
        private readonly IWorkSchedulesView _workSchedulesView;
        public ScheduleViewsController(IWorkSchedulesView workSchedulesView)
        {
            _workSchedulesView = workSchedulesView;
        }
        [Authorize(Roles ="Mentor")]
        [HttpGet("mentor-schedules")]
        public async Task<IActionResult> MentorSchedules()
        {
            var MentorId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var results = _workSchedulesView.MentorWorkScheulesViews(Guid.Parse(MentorId!));
            return Ok(results);
        }
        [Authorize(Roles ="Student")]
        [HttpGet("student-schedules")]
        public async Task<IActionResult> StudentSchedules()
        {
            var StudentId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var results = await _workSchedulesView.StudentWorkScheulesViews(Guid.Parse(StudentId!));
            return Ok(results);
        }
        [Authorize]
        [HttpGet("available-schedules")]
        public async Task<IActionResult> AvailableSchedules([FromQuery] Guid MentorId )
        {
            var results = await _workSchedulesView.SchedulesForBooking(MentorId);
            return Ok(results);
        }
    }
}
