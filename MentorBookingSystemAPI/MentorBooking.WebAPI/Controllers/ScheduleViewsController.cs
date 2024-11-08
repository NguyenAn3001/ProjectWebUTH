using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleViewsController : ControllerBase
    {
        private readonly IWorkSchedulesView _workSchedulesView;
        public ScheduleViewsController(IWorkSchedulesView workSchedulesView)
        {
            _workSchedulesView = workSchedulesView;
        }
        [HttpGet("mentor-schedules")]
        public async Task<IActionResult> MentorSchedules([FromQuery] Guid MentorId)
        {
            var results = _workSchedulesView.MentorWorkScheulesViews(MentorId);
            return Ok(results);
        }
        [HttpGet("student-schedules")]
        public async Task<IActionResult> StudentSchedules([FromQuery] Guid StudentId)
        {
            var results = await _workSchedulesView.StudentWorkScheulesViews(StudentId);
            return Ok(results);
        }
        [HttpGet("available-schedules")]
        public async Task<IActionResult> AvailableSchedules([FromQuery] Guid MentorId )
        {
            var results = await _workSchedulesView.SchedulesForBooking(MentorId);
            return Ok(results);
        }
    }
}
