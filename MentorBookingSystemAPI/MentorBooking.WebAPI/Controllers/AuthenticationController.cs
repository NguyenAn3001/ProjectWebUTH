﻿using Azure;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MentorBooking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelRequest registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RegisterModelResponse 
                { 
                    Status = "Error", Message = "Invalid data provided." 
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
    }
}
