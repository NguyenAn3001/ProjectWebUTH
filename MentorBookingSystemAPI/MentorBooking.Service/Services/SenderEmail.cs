using System.Net;
using System.Net.Mail;
using System.Text;
using MentorBooking.Service.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MentorBooking.Service.Services;

public class SenderEmail : ISenderEmail
{
    private readonly IConfiguration _configuration;

    public SenderEmail(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public Task SendEmailAsync(string email, string subject, string message, bool isBodyHtml = false)
    {
        string MailServer = _configuration["EmailSettings:MailServer"]!;
        string FromEmail = _configuration["EmailSettings:FromEmail"]!;
        string Password = _configuration["EmailSettings:Password"]!;
        int Port = int.Parse(_configuration["EmailSettings:MailPort"]!);

        var client = new SmtpClient(MailServer, Port)
        {
            Credentials = new NetworkCredential(FromEmail, Password),
            EnableSsl = true,
        };

        MailMessage mailMessage = new MailMessage(FromEmail, email, subject, message)
        {
            IsBodyHtml = isBodyHtml
        };

        return client.SendMailAsync(mailMessage);
    }
    // private string BuildEmailBody()
    // {
    //     var sb = new StringBuilder();
    //
    //     sb.Append("<div style='font-family: Arial, sans-serif; margin: 20px;'>");
    //     sb.Append("<h2 style='color: #4CAF50;'>Welcome to Mentor Booking System!</h2>");
    //     sb.Append("<p>We are excited to have you join us.</p>");
    //     sb.Append("<hr style='border-top: 1px solid #ddd;' />");
    //
    //     sb.Append("<h3>Your Booking Details:</h3>");
    //     sb.Append("<table style='width: 100%; border-collapse: collapse;'>");
    //     sb.Append("<tr style='background-color: #f2f2f2;'>");
    //     sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Mentor</th>");
    //     sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Date</th>");
    //     sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Time</th>");
    //     sb.Append("</tr>");
    //     sb.Append("<tr>");
    //     sb.Append("<td style='padding: 8px; border: 1px solid #ddd;'>John Doe</td>");
    //     sb.Append("<td style='padding: 8px; border: 1px solid #ddd;'>2024-10-28</td>");
    //     sb.Append("<td style='padding: 8px; border: 1px solid #ddd;'>10:00 AM - 11:00 AM</td>");
    //     sb.Append("</tr>");
    //     sb.Append("</table>");
    //
    //     sb.Append("<p style='margin-top: 20px;'>Need help? Contact us at <a href='mailto:support@mentorbooking.com'>support@mentorbooking.com</a>.</p>");
    //     sb.Append("<p style='color: gray; font-size: 12px;'>© 2024 Mentor Booking System. All rights reserved.</p>");
    //     sb.Append("</div>");
    //
    //     return sb.ToString();
    // }
}
