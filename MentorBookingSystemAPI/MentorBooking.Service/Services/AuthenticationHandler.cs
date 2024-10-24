using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Services
{
    public class AuthenticationHandler : IAuthenticateService
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationHandler(UserManager<Users> userManager, SignInManager<Users> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._configuration = configuration;
        }
        public async Task<RegisterModelResponse> RegisterUserAsync(RegisterModelRequest registerModel)
        {
            try
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
            catch (Exception ex)
            {
                return new RegisterModelResponse()
                {
                    Status = "500",
                    Message = ex.Message
                };
            }
        }


        private string GenerateAccessToken(Users user)
        {
            // Creates a new symmetric security key from the JWT key specified in the app _configuration.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // Sets up the signing credentials using the above security key and specifying the HMAC SHA256 algorithm.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Defines a set of claims to be included in the token.
            var claims = new List<Claim>
            {
                // Custom claim using the user's ID.
                new Claim("Myapp_User_Id", user.Id.ToString()),
                // Standard claim for user identifier, using username.
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                // Standard claim for user's email.
                new Claim(ClaimTypes.Email, user.Email),
                // Standard JWT claim for subject, using user ID.
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("issAt", DateTime.Now.ToString())
            };

            // Adds a role claim for each role associated with the user.
            user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

            // Creates a new JWT token with specified parameters including issuer, audience, claims, expiration time, and signing credentials.
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration set to 1 hour from the current time.
                signingCredentials: credentials);

            // Serializes the JWT token to a string and returns it.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];  // Prepare a buffer to hold the random bytes.
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);  // Fill the buffer with cryptographically strong random bytes.
                return Convert.ToBase64String(randomNumber);  // Convert the bytes to a Base64 string and return.
            }
        }
    }
}
