using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;

namespace MentorBooking.Service.Services
{
    public class AuthenticationHandler : IAuthenticateService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IConfirmEmailRepository _confirmEmailRepository;
        private readonly IStudentRepository _studentRepository;

        public AuthenticationHandler(IUserRepository userRepository, IConfiguration configuration, IRoleRepository roleRepository, IUserTokenRepository userTokenRepository, IConfirmEmailRepository confirmEmailRepository, IStudentRepository studentRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _userTokenRepository = userTokenRepository;
            _confirmEmailRepository = confirmEmailRepository;
            _studentRepository = studentRepository;
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
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerModel.UserName
                };
                if (registerModel.RoleName!.ToLower() == "admin")
                    return new RegisterModelResponse
                    {
                        Status = "Forbidden",
                        Message = "Creating 'Admin' role is not allowed"
                    };
                if (registerModel.RoleName!.ToLower() != "student" && registerModel.RoleName!.ToLower() != "mentor")
                {
                    return new RegisterModelResponse
                    {
                        Status = "Error",
                        Message = "Creating role is not allowed"
                    };
                }
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
                if (!await _roleRepository.RoleExistsAsync(registerModel.RoleName!))
                {
                    if (!await _roleRepository.CreateRoleAsync(registerModel.RoleName!))
                        return new RegisterModelResponse
                        {
                            Status = "Error",
                            Message = "Failed to create role"
                        };
                }

                var resultRole = await _roleRepository.AddUserToRoleAsync(users, registerModel.RoleName);
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

        public async Task<RefreshTokenModelResponse> RefreshToken(RefreshTokenModelRequest refreshTokenModel)
        {
            try
            {
                var userIdInUserTokens = await _userTokenRepository.GetUserIdByRefreshToken(refreshTokenModel.RefreshToken!);
                if (userIdInUserTokens == null)
                    return new RefreshTokenModelResponse
                    {
                        Status = "Unauthorized",
                        Message = "Invalid refresh token."
                    };
                var user = await _userRepository.FindByIdAsync(userIdInUserTokens.ToString()!);
                if (user == null)
                    return new RefreshTokenModelResponse
                    {
                        Status = "Unauthorized",
                        Message = "Invalid user."
                    };
                var accessToken = await GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken();
                await _userTokenRepository.SetAuthenticationTokenToTableAsync(user, "Local", "Refresh Token", refreshToken);
                return new RefreshTokenModelResponse
                {
                    Status = "Success",
                    Message = "Refresh token successfully.",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                return new RefreshTokenModelResponse()
                {
                    Status = "ServerError",
                    Message = ex.Message
                };
            }
        }

        public Task<string?> GetCofirmTokenAsync(ConfirmationEmailModelRequest confirmationEmailModel)
        {
            return _confirmEmailRepository.CreateEmailConfirmationTokenAsync(confirmationEmailModel.UserId.ToString());
        }

        public async Task<string> GenerateBodyMessageForConfirmationEmailAsync(
            ConfirmationEmailModelRequest confirmationEmailModel, string confirmLink)
        {
            var user = await _userRepository.FindByIdAsync(confirmationEmailModel.UserId.ToString());
            var fullName = $"{user?.LastName} {user?.FirstName}";
            return BuildConfirmationEmailBody(fullName, confirmLink);
        }

        public async Task<bool> IsEmailConfirmedAsync(LinkEmailConfirmModelRequest linkEmailConfirm)
        {
            var user = await _userRepository.FindByIdAsync(linkEmailConfirm.UserId ?? "");
            if (user == null)
                return false;
            await _confirmEmailRepository.ConfirmationEmailAsync(user);
            return true;
        }

        private async Task<string> GenerateAccessToken(Users user)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var userRoles = await _roleRepository.GetRolesByUserAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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

        private async Task<Guid?> RetrieveUsernameByRefreshToken(string refreshToken)
        {
            var user = await _userTokenRepository.GetUserIdByRefreshToken(refreshToken);
            return user;
        }
        private string BuildConfirmationEmailBody(string userName, string confirmationLink)
        {
            var sb = new StringBuilder();
            var encodedLink = HtmlEncoder.Default.Encode(confirmationLink!);
            sb.Append("<div style='font-family: Arial, sans-serif; margin: 20px;'>");
            sb.Append("<h2 style='color: #4CAF50;'>Welcome to Mentor Booking System, " + userName + "!</h2>");
            sb.Append("<p>Thank you for registering. Please confirm your email address to activate your account.</p>");
            sb.Append("<hr style='border-top: 1px solid #ddd;' />");

            sb.Append("<table role='presentation' cellspacing='0' cellpadding='0' border='0' style='margin: 20px 0;'>");
            sb.Append("<tr>");
            sb.Append("<td align='center' style='border-radius: 5px;' bgcolor='#4CAF50'>");
            sb.Append($"<a href='{encodedLink}' target='_blank' style='");
            sb.Append("font-size: 16px; font-family: Arial, sans-serif; color: white; text-decoration: none;");
            sb.Append("padding: 10px 20px; display: inline-block;'>");
            sb.Append("Confirm your email</a>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("<p style='color: gray; font-size: 12px;'>");
            sb.Append("If you did not create this account, you can ignore this email.</p>");

            sb.Append("<p style='color: gray; font-size: 12px;'>");
            sb.Append("© 2024 Mentor Booking System. All rights reserved.</p>");

            sb.Append("</div>");

            return sb.ToString();
        }

    }
}
