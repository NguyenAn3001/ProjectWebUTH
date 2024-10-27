using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using MentorBooking.Repository.Data;

namespace MentorBooking.Repository.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<Roles> _roleManager;
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public RoleRepository(RoleManager<Roles> roleManager, UserManager<Users> userManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> AddUserToRoleAsync(Users user, string roleName)
        {
            if (roleName.ToLower() == "mentor")
            {
                var studentRespone = _dbContext.Students.Where(x => x.StudentId == user.Id).ToList();
                if (studentRespone.Count > 0)
                {
                    _dbContext.Students.RemoveRange(studentRespone);
                }
            }
            var userWithRoles = _dbContext.UserRoles.Where(x => x.UserId == user.Id).ToList();
            if (userWithRoles.Count() > 0)
            {
                _dbContext.RemoveRange(userWithRoles);
                await _dbContext.SaveChangesAsync();
            }
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new Roles { Name = roleName });
            return result.Succeeded;
        }

        public async Task<Users?> FindUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IList<string>> GetRolesByUserAsync(Users users)
        {
            return await _userManager.GetRolesAsync(users);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
