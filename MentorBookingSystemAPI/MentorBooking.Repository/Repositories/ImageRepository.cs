using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Repository.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ImageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> UpdateImageAsync(Users user, string imageUrl)
    {
        try
        {
            user.Image = imageUrl;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteImageAsync(Users user)
    {
        try
        {
            user.Image = "";
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<string?> GetImageUrlAsync(Guid userId)
    {
        try
        {
            var users = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (users == null || string.IsNullOrEmpty(users.Image)) return null; // fix bug: when image deleted but it get request scheme
            return users.Image;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }    
    }
}