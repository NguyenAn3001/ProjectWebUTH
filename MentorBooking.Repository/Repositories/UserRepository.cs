using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(UserManager<Users> userManager,ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<bool> CheckPasswordUserAsync(Users user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateUserAsync(Users user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<Users?> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<Users?> FindByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public List<Users>? GetAllUser()
        {
            var listUser = _dbContext.Users.ToList();
            if(listUser == null) { return null; }
            return listUser;
        }
    }
}
