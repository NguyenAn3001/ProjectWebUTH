using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IConfirmEmailRepository
{
    Task<string?> CreateEmailConfirmationTokenAsync(string userId);
    Task<bool> ConfirmationEmailAsync(Users user);
}