using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Enum;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

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

        [HttpGet("Search")]
        public IActionResult SearchMentor([FromQuery] int page = 1, [FromQuery] int pageSize=10,[FromQuery]string? searchText="",[FromQuery]string? sortBy="")
        {
            var query = _mentorServices.GetMentorBySearchText(searchText);
            var totalCount=query.Count();
            var totalPages= (int)Math.Ceiling((double)totalCount / pageSize);
            query = (List<MentorSearchingResponse>)query.Skip((page - 1) * pageSize).Take(pageSize);

            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Articles = query.ToList()
            };
            
            return Ok(result);
        }
    }
}
