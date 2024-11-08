using System.Security.Claims;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingMentorController : Controller
    {
        private readonly IBookingMentorService _bookingMentorService;
        private readonly IAcceptBookingSession _acceptBookingSession;
        public BookingMentorController(IBookingMentorService bookingMentorService,IAcceptBookingSession acceptBookingSession)
        {
            _bookingMentorService = bookingMentorService;
            _acceptBookingSession = acceptBookingSession;
        }
        [Authorize(Roles = "Student")]
        [HttpPost("booking-mentor")]
        public async Task<IActionResult> BookingMentor([FromBody] MentorSupportSessionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookingResponse= await _bookingMentorService.BookingMentor(userId!, request);
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
        [HttpGet("get-booking-mentor-session")]
        public async Task<IActionResult> getBookingMentorSession([FromQuery] Guid SessionId)
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
        [HttpGet("get-unaccept-booking")]
        public async Task<IActionResult> getAllUnacceptBooking([FromQuery] Guid MentorId)
        {
            if(MentorId==Guid.Empty)
                return BadRequest(new { message = "SessionId is required" });
            var getSessionResponse = await _acceptBookingSession.GetAllSessionUnAccept(MentorId);
            return Ok(getSessionResponse);
        }
        // [Authorize(Roles = "Mentor")]
        [AllowAnonymous]
        [HttpGet("accept-booking")]
        public async Task<IActionResult> AcceptBooking([FromQuery] Guid SessionId, bool accept)
        {
            if (SessionId == Guid.Empty)
                return BadRequest(new { message = "SessionId is required" });
            var getSessionResponse = await _acceptBookingSession.AcceptSession(SessionId,accept);
            return Ok(getSessionResponse);
        }
    }
}
