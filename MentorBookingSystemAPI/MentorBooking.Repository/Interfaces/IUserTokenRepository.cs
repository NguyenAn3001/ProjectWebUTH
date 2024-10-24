using MentorBooking.Repository.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Interfaces
{
    public interface IUserTokenRepository
    {
        Task<IdentityResult> SetAuthenticationTokenToTableAsync(Users user, string provider, string nameOfToken, string valueToken);
        Task<IdentityResult> RemoveAuthenticationTokenToTableAsync(Users user, string provider, string nameOfToken);
    }
}
