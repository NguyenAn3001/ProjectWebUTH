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
            var userToken = await _dbContext.UserTokens.SingleOrDefaultAsync(x => x.Value == refreshToken && x.Expired > DateTime.Now);
            return userToken?.UserId;
        }

        public async Task<IdentityResult> SetAuthenticationTokenToTableAsync(Users users, string provider, string nameOfToken, string valueToken)
        {

            var result = await _userManager.SetAuthenticationTokenAsync(users, provider, nameOfToken, valueToken);
            if (result.Succeeded)
            {
                var userToken = _dbContext.UserTokens.SingleOrDefault(x => x.UserId == users.Id && x.LoginProvider == provider && x.Name == nameOfToken);
                if (userToken != null)
                {
                    userToken.Expired = DateTime.Now.AddDays(1);
                    _dbContext.SaveChanges();
                }
            }
            return result;
        }
    }
}
