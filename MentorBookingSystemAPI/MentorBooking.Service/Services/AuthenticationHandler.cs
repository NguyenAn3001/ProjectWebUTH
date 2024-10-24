using Azure;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Services
{
    public class AuthenticationHandler : IAuthenticateService
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<Users> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserTokenRepository _userTokenRepository;

        public AuthenticationHandler(IUserRepository userRepository, SignInManager<Users> signInManager, IConfiguration configuration, IRoleRepository roleRepository, IUserTokenRepository userTokenRepository)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _userTokenRepository = userTokenRepository;
        }
        public async Task<RegisterModelResponse> RegisterUserAsync(RegisterModelRequest registerModel)
        {
            try
            {
                Users? user = await _userRepository.FindByUserNameAsync(registerModel.UserName!);
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
                var result = await _userRepository.CreateUserAsync(users, registerModel.Password!);
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
                    UserId = users.Id,
                    Status = "Success",
                    Message = "User created successfully!"
                };
            }
            catch (Exception ex)
            {
                return new RegisterModelResponse()
                {
                    Status = "ServerError",
                    Message = ex.Message
                };
            }
        }

        public async Task<SettingRoleModelResponse> SettingRoleAsync(SettingRoleModelRequest settingRoleModel)
        {
            try
            {
                var user = await _roleRepository.FindUserByIdAsync(settingRoleModel.UserId!);
                if (user == null)
                    return new SettingRoleModelResponse
                    {
                        Status = "Error",
                        Message = "User not found"
                    };
                if (!await _roleRepository.RoleExistsAsync(settingRoleModel.RoleName!))
                {
                    if (!await _roleRepository.CreateRoleAsync(settingRoleModel.RoleName!))
                        return new SettingRoleModelResponse
                        {
                            Status = "Error",
                            Message = "Failed to create role"
                        };
                }
                if (settingRoleModel.RoleName!.ToLower() == "admin")
                    return new SettingRoleModelResponse
                    {
                        Status = "Forbidden",
                        Message = "Creating 'Admin' role is not allowed"
                    };
                var result = await _roleRepository.AddUserToRoleAsync(user, settingRoleModel.RoleName);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return new SettingRoleModelResponse
                    {
                        Status = "Error",
                        Message = $"User creation failed: {errors}"
                    };
                }
                return new SettingRoleModelResponse
                {
                    Status = "Success",
                    Message = "Setting role successfully."
                };
            }
            catch (Exception ex)
            {
                return new SettingRoleModelResponse()
                {
                    Status = "ServerError",
                    Message = ex.Message
                };
            }
        }
        public async Task<LoginModelResponse> Login(LoginModelRequest loginModel)
        {
            try
            {
                var user = await _userRepository.FindByUserNameAsync(loginModel.UserName!);
                if (user == null || !await _userRepository.CheckPasswordUserAsync(user, loginModel.Password!))
                    return new LoginModelResponse
                    {
                        Status = "Unauthorized",
                        Message = "Invalid username or password."
                    };

                string accessToken = await GenerateAccessToken(user);
                string refreshToken = GenerateRefreshToken();
                await _userTokenRepository.SetAuthenticationTokenToTableAsync(user, "Local", "Refresh Token", refreshToken);
                return new LoginModelResponse
                {
                    UserId = user.Id,
                    Status = "Success",
                    Message = "Login successfully.",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                return new LoginModelResponse()
                {
                    Status = "ServerError",
                    Message = ex.Message
                };
            }
        }
        private async Task<string> GenerateAccessToken(Users user)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var userRoles = await _roleRepository.GetRolesByUserAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName!),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<LogoutModelResponse> Logout(LogoutModelRequest logoutModel)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(logoutModel.UserId!);
                if (user == null)
                    return new LogoutModelResponse
                    {
                        Status = "NotFound",
                        Message = "User not found."
                    };

                await _userTokenRepository.RemoveAuthenticationTokenToTableAsync(user, "Local", "Refresh Token");
                return new LogoutModelResponse
                {
                    Status = "Success",
                    Message = "Logged out successfully."
                };
            }
            catch (Exception ex)
            {
                return new LogoutModelResponse()
                {
                    Status = "ServerError",
                    Message = ex.Message
                };
            }
        }
    }
}
