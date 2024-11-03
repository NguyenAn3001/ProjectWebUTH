using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingMentorController : Controller
    {
        private readonly IBookingMentorService _bookingMentorService;
        public BookingMentorController(IBookingMentorService bookingMentorService)
        {
            _bookingMentorService = bookingMentorService;
        }
        [HttpPost("booking-mentor")]
        public async Task<IActionResult> BookingMentor([FromBody] MentorSupportSessionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
            }
            var bookingResponse= await _bookingMentorService.BookingMentor(request);
            return bookingResponse.Status switch
            {
                "Error" => BadRequest(new { status = bookingResponse.Status, message = bookingResponse.Message }),
                _ => Ok(new { status = bookingResponse.Status, message = bookingResponse.Message })
            };
        }
        [HttpPost("delete-booking-mentor-session")]
        public async Task<IActionResult> deleteBookingMentorSession([FromBody] Guid SessionId)
        {
            if (SessionId == Guid.Empty)
                return BadRequest(new { message = "SessionId is required" });
            var deleteSessionResponse = await _bookingMentorService.DeleteMentorSupportSessionAsync(SessionId);
            return deleteSessionResponse.Status switch
            {
                "Error" => BadRequest(new
                {
                    Status = deleteSessionResponse.Status,
                    Message = deleteSessionResponse.Message
                }),
                _ => Ok(new
                {
                    Status = deleteSessionResponse.Status,
                    Message = deleteSessionResponse.Message
                })
            };
        }
        [HttpPost("get-booking-mentor-session")]
        public async Task<IActionResult> getBookingMentorSession([FromBody] Guid SessionId)
        {
            if (SessionId == Guid.Empty)
                return BadRequest(new { message = "SessionId is required" });
            var getSessionResponse = await _bookingMentorService.GetMentorSupportSessionAsync(SessionId);
            return getSessionResponse.Status switch
            {
                "Error" => BadRequest(new
                {
                    Status = getSessionResponse.Status,
                    Message = getSessionResponse.Message
                }),
                _ => Ok(new
                {
                    Status = getSessionResponse.Status,
                    Message = getSessionResponse.Message
                })
            };
        }
    }
}
