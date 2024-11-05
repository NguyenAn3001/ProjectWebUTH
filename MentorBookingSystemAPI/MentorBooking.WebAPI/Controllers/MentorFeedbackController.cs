using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using MentorBooking.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MentorFeedbackController : ControllerBase
    {
        private readonly IMentorFeedbackService _feedbackService;
        public MentorFeedbackController(IMentorFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddMentorFeedBack([FromBody]StudentCommentRequest studentComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
            }
            var CommentResponse = await _feedbackService.AddStudentCommentAsync(studentComment);
            return CommentResponse.Status switch
            {
                "Error" => BadRequest(new { status = CommentResponse.Status, message = CommentResponse.Message }),
                _ => Ok(new { status = CommentResponse.Status, message = CommentResponse.Message })
            };
        }
        [HttpDelete("delete-comment")]
        public async Task<IActionResult> DeleteMentorFeedBack([FromQuery] Guid MentorFeedbackId)
        {
            if (MentorFeedbackId == Guid.Empty)
                return BadRequest(new { message = "MentorFeedbackId is required" });
            var deleteFeedBackResponse = await _feedbackService.DeleteMentorFeedbackAsync(MentorFeedbackId);
            return deleteFeedBackResponse.Status switch
            {
                "Error" => BadRequest(new
                {
                    Status = deleteFeedBackResponse.Status,
                    Message = deleteFeedBackResponse.Message
                }),
                _ => Ok(new
                {
                    Status = deleteFeedBackResponse.Status,
                    Message = deleteFeedBackResponse.Message
                })
            };
        }
        [HttpGet("get-feedback")]
        public async Task<IActionResult> GetMentorFeedback([FromQuery] Guid MentorFeedbackId)
        {
            if (MentorFeedbackId == Guid.Empty)
                return BadRequest(new { message = "MentorFeedbackId is required" });
            var getFeedBackResponse = await _feedbackService.GetMentorFeedback(MentorFeedbackId);
            return getFeedBackResponse.Status switch
            {
                "Error" => BadRequest(new
                {
                    Status = getFeedBackResponse.Status,
                    Message = getFeedBackResponse.Message
                }),
                _ => Ok(new
                {
                    Status = getFeedBackResponse.Status,
                    Message = getFeedBackResponse.Message,
                    Data=getFeedBackResponse.Data
                })
            };
        }
        [HttpGet("list-feedback")]
        public async Task<IActionResult> GetALlMentorFeedBack([FromQuery] Guid MentorId)
        {
            if (MentorId == Guid.Empty)
                return BadRequest(new { message = "MentorId is required" });
            var listMentorFeedback = await _feedbackService.GetAllMentorFeedback(MentorId);
            return Ok(listMentorFeedback);
        }
        [HttpPut("update-feedback")]
        public async Task<IActionResult> UpdateMentorFeedBack([FromBody] StudentCommentRequest studentComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });
            }
            var updateResponse = await _feedbackService.UpdateMentorFeedbackAsync(studentComment);
            return updateResponse.Status switch
            {
                "Error" => BadRequest(new { status = updateResponse.Status, message = updateResponse.Message }),
                _ => Ok(new { status = updateResponse.Status, message = updateResponse.Message })
            };
        }
    }
}
