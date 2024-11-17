using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Enum;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace MentorBooking.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchAndSortController : ControllerBase
    {
        private readonly ISearchAndSortService _mentorServices;

        public SearchAndSortController(ISearchAndSortService mentorServices)
        {
            _mentorServices = mentorServices;
        }
        [Authorize]
        [HttpGet("Search")]
        public IActionResult SearchMentor([FromQuery] int page = 1, [FromQuery] int pageSize=10,[FromQuery]string? searchText="",[FromQuery]string? sortBy="")
        {
            List<MentorSearchingResponse> querySearch = _mentorServices.GetMentorBySearchText(searchText);
            List<MentorSearchingResponse> query = _mentorServices.GetSortMentor(querySearch, sortBy);
            if (query != null)
            {
                var totalCount = query.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                query = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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
            return Ok(null);
        }
    }
}
