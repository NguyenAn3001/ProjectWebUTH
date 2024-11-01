using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MentorController : ControllerBase
    {
        private readonly ISearchAndSortService _mentorServices;
        private readonly ISchedulesMentor _schedulesMentor;

        public MentorController(ISearchAndSortService mentorServices, ISchedulesMentor schedulesMentor)
        {
            _mentorServices = mentorServices;
            _schedulesMentor = schedulesMentor;
        }

        [HttpPost("Search")]
        public IActionResult SearchMentor(string? searchText, string? sortOptions)
        {
            List<MentorSearchingResponse> results = _mentorServices.GetMentorBySearchText(searchText);
            List<MentorSearchingResponse> sortResults = _mentorServices.GetSortMentor(results, sortOptions);
            return Ok(results);
        }
    }
}
