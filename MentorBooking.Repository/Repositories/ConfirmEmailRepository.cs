using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MentorBooking.Repository.Repositories;

public class ConfirmEmailRepository : IConfirmEmailRepository
{
    private readonly UserManager<Users> _userManager;

    public ConfirmEmailRepository(UserManager<Users> userManager)
    {
        _userManager = userManager;
    }
    public async Task<string?> CreateEmailConfirmationTokenAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);
            return token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<bool> ConfirmationEmailAsync(Users user)
    {
        try
        {
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}