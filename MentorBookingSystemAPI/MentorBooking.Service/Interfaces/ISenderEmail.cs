namespace MentorBooking.Service.Interfaces;

public interface ISenderEmail
{
    Task SendEmailAsync(string email, string subject, string message, bool isBodyHtml = false);
}