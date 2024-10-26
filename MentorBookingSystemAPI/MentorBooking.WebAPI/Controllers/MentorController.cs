using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Enum;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MentorController : ControllerBase
    {
        private readonly IMentorServices _mentorServices;

        public MentorController(IMentorServices mentorServices)
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
