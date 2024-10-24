using MentorBooking.Repository.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<Users?> FindUserByIdAsync(string userId);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> CreateRoleAsync(string roleName);
        Task<IdentityResult> AddUserToRoleAsync(Users user, string roleName);
        Task<IList<string>> GetRolesByUserAsync(Users users);
    }

}
