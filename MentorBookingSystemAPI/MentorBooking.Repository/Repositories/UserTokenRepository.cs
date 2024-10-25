using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Repository.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public UserTokenRepository(UserManager<Users> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> RemoveAuthenticationTokenToTableAsync(Users user, string provider, string nameOfToken)
        {
            return await _userManager.RemoveAuthenticationTokenAsync(user, provider, nameOfToken);
        }

        public async Task<Guid?> GetUserIdByRefreshToken(string refreshToken)
        {
            var userToken = await _dbContext.UserTokens.SingleOrDefaultAsync(x => x.Value == refreshToken);
            return userToken?.UserId;
        }

        public async Task<IdentityResult> SetAuthenticationTokenToTableAsync(Users users, string provider, string nameOfToken, string valueToken)
        {
            return await _userManager.SetAuthenticationTokenAsync(users, provider, nameOfToken, valueToken);    
        }
    }
}
