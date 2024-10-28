using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Enum;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ISearchAndSortService _mentorServices;

        public StudentController(ISearchAndSortService mentorServices)
        {
            _mentorServices = mentorServices;
        }

        [HttpPost("Search")]
        public IActionResult SearchMentor(string? searchText, SortOptions sortOptions)
        {
            List<MentorSearchingResponse> results = _mentorServices.GetMentorBySearchText(searchText);
            List<MentorSearchingResponse> sortResults = _mentorServices.GetSortMentor(results, sortOptions);
            return Ok(results);
        }
    }
}
