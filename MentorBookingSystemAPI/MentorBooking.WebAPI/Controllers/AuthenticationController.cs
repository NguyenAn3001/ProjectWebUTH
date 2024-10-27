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

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
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
        [Authorize(Roles = "Admin, Student")]
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
        public async Task<IActionResult> Logout([FromBody] LogoutModelRequest logoutRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new LoginModelResponse
                {
                    Status = "Error",
                    Message = "Invalid data provided."
                });
            var logoutResponse = await _authenticateService.Logout(logoutRequest);
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
