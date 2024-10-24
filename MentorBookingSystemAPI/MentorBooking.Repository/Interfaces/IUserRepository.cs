using MentorBooking.Repository.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(Users user, string password);
        Task<Users?> FindByUserNameAsync(string userName);
    }
}
