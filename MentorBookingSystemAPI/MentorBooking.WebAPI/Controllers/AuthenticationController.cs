using System.Security.Claims;
using Azure.Messaging;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MentorBooking.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly ISenderEmail _senderEmail;

        public AuthenticationController(IAuthenticateService authenticateService, ISenderEmail senderEmail)
        {
            _authenticateService = authenticateService;
            _senderEmail = senderEmail;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelRequest registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RegisterModelResponse
                {
                    Status = "Error",
                    Message = "Invalid data provided."
                });
            }
            var registerResponse = await _authenticateService.RegisterUserAsync(registerModel);
            return registerResponse.Status switch
            {
                "Error" => BadRequest(registerResponse),
                "ServerError" => StatusCode(StatusCodes.Status500InternalServerError, registerResponse),
                _ => Ok(registerResponse)
            };
            //if (registerResponse.Status == "Error")
            //    return StatusCode(StatusCodes.Status400BadRequest, registerResponse);
            //if (registerResponse.Status == "500")
            //    return StatusCode(StatusCodes.Status500InternalServerError, registerResponse);
            //return Ok(registerResponse);
        }
        [Authorize]
        [HttpPost("send-confirm-email")]
        public async Task<IActionResult> SendConfirmEmail([FromBody] ConfirmationEmailModelRequest confirmEmailModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var confirmToken = await _authenticateService.GetConfirmTokenAsync(Guid.Parse(userId));
            var confirmLink = Url.Action(action: "ConfirmEmail", controller: "Authentication", values: new { UserId = userId, Token = confirmToken }, protocol: HttpContext.Request.Scheme);
            await _senderEmail.SendEmailAsync(email: confirmEmailModel.EmailGetConfirmToken!, subject: "Confirm Your Account", message: await _authenticateService.GenerateBodyMessageForConfirmationEmailAsync(Guid.Parse(userId), confirmLink ?? ""), isBodyHtml: true);
            return Ok(new ConfirmationEmailModelResponse()
            {
                Status = "Success",
                Message = "Send confirmation email successfully."
            });
        }
        [Authorize]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string UserId, string Token)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token))
            {
                return BadRequest(new ConfirmationEmailModelResponse()
                {
                    Status = "Error",
                    Message = "The link is Invalid or Expired."
                });
            }

            var linkEmailConfirmRequest = new LinkEmailConfirmModelRequest()
            {
                UserId = UserId,
                Token = Token
            };
            await _authenticateService.IsEmailConfirmedAsync(linkEmailConfirmRequest);
            return Ok(new ConfirmationEmailModelResponse()
            {
                Status = "Success",
                Message = "Confirm email success."
            });
        }
        [Authorize(Roles = "Student")]
        [HttpPost("setting-role")]
        public async Task<IActionResult> SettingRoleForUser([FromBody] SettingRoleModelRequest settingRoleModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new RegisterModelResponse
                {
                    Status = "Error",
                    Message = "Invalid data provided."
                });
            var settingRoleResponse = await _authenticateService.SettingRoleAsync(settingRoleModel);
            return settingRoleResponse.Status switch
            {
                "Error" => BadRequest(settingRoleResponse),
                "Forbidden" => StatusCode(StatusCodes.Status403Forbidden, settingRoleResponse),
                "ServerError" => StatusCode(StatusCodes.Status500InternalServerError, settingRoleResponse),
                _ => Ok(settingRoleResponse)
            };
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelRequest loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RegisterModelResponse
                {
                    Status = "Error",
                    Message = "Invalid data provided."
                });
            }
            var loginResponse = await _authenticateService.Login(loginModel);
            return loginResponse.Status switch
            {
                "Unauthorized" => Unauthorized(loginResponse),
                "ServerError" => StatusCode(StatusCodes.Status500InternalServerError, loginResponse),
                _ => Ok(loginResponse)
            };
        }
        [Authorize(Roles = "Student, Mentor, Admin")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            if (!ModelState.IsValid)
                return BadRequest(new LoginModelResponse
                {
                    Status = "Error",
                    Message = "Invalid data provided."
                });
            var logoutResponse = await _authenticateService.Logout(Guid.Parse(userId));
            return logoutResponse.Status switch
            {
                "NotFound" => NotFound(logoutResponse),
                "ServerError" => StatusCode(StatusCodes.Status500InternalServerError, logoutResponse),
                _ => Ok(logoutResponse)
            };
        }
        [Authorize(Roles = "Student, Mentor, Admin")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModelRequest refreshTokenModelRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new LoginModelResponse
                {
                    Status = "Error",
                    Message = "Invalid data provided."
                });
            var refreshTokenResponse = await _authenticateService.RefreshToken(refreshTokenModelRequest);
            return refreshTokenResponse.Status switch
            {
                "NotFound" => NotFound(refreshTokenResponse),
                "ServerError" => StatusCode(StatusCodes.Status500InternalServerError, refreshTokenResponse),
                _ => Ok(refreshTokenResponse)
            };
        }
    }
}
