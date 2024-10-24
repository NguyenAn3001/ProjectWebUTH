using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly UserManager<Users> _userManager;

        public UserTokenRepository(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> SetAuthenticationTokenToTableAsync(Users users, string provider, string nameOfToken, string valueToken)
        {
            return await _userManager.SetAuthenticationTokenAsync(users, provider, nameOfToken, valueToken);    
        }
    }
}
