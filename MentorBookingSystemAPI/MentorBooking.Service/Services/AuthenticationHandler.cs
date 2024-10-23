using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Services
{
    public class AuthenticationHandler : IAuthenticateService
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        public AuthenticationHandler(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<RegisterModelResponse> RegisterUserAsync(RegisterModelRequest registerModel)
        {
            Users? user = await _userManager.FindByNameAsync(registerModel.UserName);
            if (user != null)
            {
                return new RegisterModelResponse { Status = "Error", Message = "User already exists!" };
            }
            Users users = new Users()
            {
                Email = registerModel.Email,
                PhoneNumber = registerModel.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.UserName
            };
            var result = await _userManager.CreateAsync(users, registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new RegisterModelResponse
                {
                    Status = "Error",
                    Message = $"User creation failed: {errors}"
                };
            }
            return new RegisterModelResponse
            {
                Status = "Success",
                Message = "User created successfully!"
            };
        }
    }
}
