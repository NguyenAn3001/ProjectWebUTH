using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentorBooking.WebAPI.Controllers;
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly ISenderEmail _senderEmail;

    public EmailController(ISenderEmail senderEmail)
    {
        _senderEmail = senderEmail;
    }
    [Authorize]
    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail(EmailMessageModelRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _senderEmail.SendEmailAsync(request.ToMailAddress!, request.Subject!, request.Body!);
        return Ok(new { message = "Email sent successfully!" });
    }
}