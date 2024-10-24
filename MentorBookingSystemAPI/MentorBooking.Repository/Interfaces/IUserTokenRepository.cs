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
        Task<IdentityResult> SetAuthenticationTokenToTableAsync(Users users, string provider, string nameOfToken, string valueToken);
    }
}
