using MentorBooking.Repository.Entities;
using Microsoft.AspNetCore.Identity;

namespace MentorBooking.Repository.Interfaces
{
    public interface IUserTokenRepository
    {
        Task<IdentityResult> SetAuthenticationTokenToTableAsync(Users user, string provider, string nameOfToken, string valueToken);
        Task<IdentityResult> RemoveAuthenticationTokenToTableAsync(Users user, string provider, string nameOfToken);
        Task<Guid?> GetUserIdByRefreshToken(string refreshToken);
    }
}
