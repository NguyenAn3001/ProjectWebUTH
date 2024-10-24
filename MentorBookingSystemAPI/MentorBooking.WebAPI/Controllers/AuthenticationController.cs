using Azure;
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
            this._authenticateService = authenticateService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelRequest registerModel)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new RegisterModelResponse { Status = "Error", Message = "Invalid data provided." });
            }
            var registerResponse = await _authenticateService.RegisterUserAsync(registerModel);
            if (registerResponse.Status == "Error")
                return StatusCode(StatusCodes.Status400BadRequest, registerResponse);
            if (registerResponse.Status == "500")
                return StatusCode(StatusCodes.Status500InternalServerError, registerResponse);
            return Ok(registerResponse);
        }
    }
}
