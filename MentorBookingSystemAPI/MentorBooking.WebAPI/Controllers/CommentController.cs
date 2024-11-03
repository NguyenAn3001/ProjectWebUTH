using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using MentorBooking.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMentorFeedbackService _feedbackService;
        public CommentController(IMentorFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpPost("add-comment")]
        public async Task<IActionResult> CommentStudent([FromBody]StudentCommentRequest studentComment)
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
        [HttpPost("delete-comment")]
        public async Task<IActionResult> DeleteCommentStudent([FromBody] Guid MentorFeedbackId)
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
        [HttpPost("get-feedback")]
        public async Task<IActionResult> GetFeedback([FromBody] Guid MentorFeedbackId)
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
                    Message = getFeedBackResponse.Message
                })
            };
        }
    }
}
