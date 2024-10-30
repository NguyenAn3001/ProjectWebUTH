using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IImageRepository
{
    Task<bool> UpdateImageAsync(Users user, string imageUrl);
    Task<bool> DeleteImageAsync(Users user);
    Task<string?> GetImageUrlAsync(Guid userId);
}