using System.Security.Claims;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Enum;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MentorController : ControllerBase
    {
        private readonly IMentorServices _mentorServices;
        private readonly ISchedulesMentor _schedulesMentor;

        public MentorController(IMentorServices mentorServices, ISchedulesMentor schedulesMentor)
        {
            _mentorServices = mentorServices;
            _schedulesMentor = schedulesMentor;
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
