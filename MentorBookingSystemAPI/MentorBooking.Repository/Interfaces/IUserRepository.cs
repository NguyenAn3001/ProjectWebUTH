using MentorBooking.Repository.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
        Task<bool> CheckPasswordUserAsync(Users user, string password);
        Task<Users?> FindByIdAsync(string id);
        List<Users>? GetAllUser();
    }
}
