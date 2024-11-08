using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MentorBooking.Service.Services
{
    public class BookingMentorService : IBookingMentorService
    {
        private readonly IMentorSupportSessionRepository _sessionRepository;
        private readonly IMentorWorkScheduleRepository _workScheduleRepository;
        private readonly ISchedulesAvailableRepository _schedulesAvailableRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISenderEmail _senderEmail;

        public BookingMentorService(IMentorSupportSessionRepository sessionRepository,
            IMentorWorkScheduleRepository workScheduleRepository,
            ISchedulesAvailableRepository schedulesAvailableRepository, IUserRepository userRepository,
            ISenderEmail senderEmail)
        {
            _sessionRepository = sessionRepository;
            _workScheduleRepository = workScheduleRepository;
            _schedulesAvailableRepository = schedulesAvailableRepository;
            _userRepository = userRepository;
            _senderEmail = senderEmail;
        }
        public async Task<ApiResponse> BookingMentor(MentorSupportSessionRequest request, string userId)
        {
            if (request.dateBookings.Count() != request.SessionCount)
            {
                return new ApiResponse
                {
                    Status = "Error",
                    Message = "Numbers of session cant different Session count"
                };
            }

            foreach (var unavailable in request.dateBookings)
            {
                if (await _workScheduleRepository.CheckWorkScheduleAsync(unavailable))
                {
                    return new ApiResponse
                    {
                        Status = "Error",
                        Message = "This Schedule is not availabe"
                    };
                }
            }
            var checkGroupId = await _sessionRepository.GetMentorSupportSessionByGroupIdAsync(request.GroupId);
            if(checkGroupId!=null)
            {
                return new ApiResponse
                {
                    Status = "Error",
                    Message = "Your group is joined a session"
                };
            }    
            var mentorSupportSession = new MentorSupportSession()
            {
                SessionId = Guid.NewGuid(),
                MentorId = request.MentorId,
                SessionCount = request.SessionCount,
                PointsPerSession = request.PointPerSession,
                GroupId = request.GroupId,
                TotalPoints = request.SessionCount * request.PointPerSession,
                SessionConfirm = false
            };
            if (!await _sessionRepository.AddMentorSupportSessionAsync(mentorSupportSession))
            {
                return new ApiResponse
                {
                    Status = "Error",
                    Message = "This is some wrong in support session"
                };
            }

            foreach (var dateBooking in request.dateBookings)
            {
                var mentorWorkSchedule = new MentorWorkSchedule()
                {
                    SessionId = mentorSupportSession.SessionId,
                    UnavailableDate = false,
                    ScheduleId = Guid.NewGuid(),
                    ScheduleAvailableId = dateBooking
                };
                if (!await _workScheduleRepository.AddMentorWordScheduleAsync(mentorWorkSchedule))
                {
                    return new ApiResponse
                    {
                        Status = "Error",
                        Message = "This is some wrong in mentor work schedule"
                    };
                }
            }

            var mentor = await _userRepository.FindByIdAsync(mentorSupportSession.MentorId.ToString());
            var student = await _userRepository.FindByIdAsync(userId);
            await _senderEmail.SendEmailAsync(student?.Email!, "Booking Confirmation - Awaiting Mentor Approval",
                BuildStudentBookingWaitEmailBody(request.dateBookings,
                    mentorName: $"{mentor!.FirstName} {mentor.LastName}"), isBodyHtml: true);
            await _senderEmail.SendEmailAsync(mentor?.Email!, "Booking Confirmation - Have new booking",
                BuildMentorAcceptEmailBody(mentorSupportSession.SessionId,
                    studentName: $"{student?.FirstName} {student?.LastName}",
                    mentorName: $"{mentor!.FirstName} {mentor.LastName}", request.dateBookings), isBodyHtml: true);
            return new ApiResponse()
            {
                Status = "Success",
                Message = "Booking is success",
                Data = new MentorSupportSessionResponse()
                {
                    SessionId=mentorSupportSession.SessionId,
                    MentorId=mentorSupportSession.MentorId,
                    GroupId=mentorSupportSession.GroupId,
                    SessionCount=mentorSupportSession.SessionCount,
                    PointPerSession=mentorSupportSession.PointsPerSession,
                    TotalPoint=mentorSupportSession.TotalPoints,
                    SessionConfirm=mentorSupportSession.SessionConfirm
                }
            };
        }


        public async Task<ApiResponse> DeleteMentorSupportSessionAsync(Guid SessionId)
        {
            var existMentorSession = await _sessionRepository.GetMentorSupportSessionAsync(SessionId);
            var existWorkSchedule = _workScheduleRepository.GetMentorWorkSchedule(SessionId).ToList();
            if (existMentorSession == null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Mentor support session is not found"
                };
            }

            if (existWorkSchedule.Count() == 0)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Work Schedule session is not found"
                };
            }

            var deleteWorkSChedule = await _workScheduleRepository.DeleteMentorWorkScheduleAsync(SessionId);
            var deleteMentorSession = await _sessionRepository.DeleteMentorSupportSessionAsync(SessionId);
            if (!deleteMentorSession)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Mentor support session is deleted or missing"
                };
            }
            if(!deleteWorkSChedule)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Work Schedule session is deleted or missing"
                };
            }

            return new ApiResponse()
            {
                Status = "Success",
                Message = "Mentor support session and Work schedule session is deleted"
            };
        }
        public List<ApiResponse>? GetAllMentorSupportSessionAsync(Guid MentorId)
        {
            List<ApiResponse>? result = new List<ApiResponse>();
            var listSession = _sessionRepository.GetAllMentorSupportSessionAsync(MentorId);
            if (listSession == null)
            {
                var response = new ApiResponse()
                {
                    Status = "Error",
                    Message = "Mentor support session is not found"
                };
                result.Add(response);
                return result;
            }

            foreach (var item in listSession)
            {
                var mentorSession = new ApiResponse()
                {
                    Status = "Success",
                    Message = "Mentor support session is found",
                    Data = new MentorSupportSessionResponse()
                    {
                        SessionId = item.SessionId,
                        MentorId = item.MentorId,
                        GroupId = item.GroupId,
                        PointPerSession = item.PointsPerSession,
                        SessionCount = item.SessionCount,
                        TotalPoint = item.TotalPoints,
                        SessionConfirm = item.SessionConfirm
                    }
                };
                result.Add(mentorSession);
            }

            return result;
        }
        public async Task<ApiResponse> GetMentorSupportSessionAsync(Guid SessionId)
        {
            var existMentorSession = await _sessionRepository.GetMentorSupportSessionAsync(SessionId);
            if (existMentorSession == null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Mentor support session is not found"
                };
            }

            return new ApiResponse()
            {
                Status = "Success",
                Message = "Mentor support session is found",
                Data = new MentorSupportSessionResponse()
                {
                    SessionId= existMentorSession.SessionId,
                    MentorId= existMentorSession.MentorId,
                    GroupId=existMentorSession.GroupId,
                    PointPerSession=existMentorSession.PointsPerSession,
                    SessionCount=existMentorSession.SessionCount,
                    TotalPoint=existMentorSession.TotalPoints,
                    SessionConfirm=existMentorSession.SessionConfirm
                }
            };
        }

        private string BuildStudentBookingWaitEmailBody(List<Guid> scheduleAvailableId, string mentorName)
        {
            var sessionTimes = scheduleAvailableId.Select(scheduleId =>
                    _schedulesAvailableRepository.GetSchedulesAvailableAsync(scheduleId).GetAwaiter().GetResult()!)
                .ToList();
            var sb = new StringBuilder();
            // General Header and Welcome Message
            sb.Append("<div style='font-family: Arial, sans-serif; margin: 20px;'>");
            sb.Append("<h2 style='color: #4CAF50;'>Your booking has been successfully send to, " + mentorName +
                      "</h2>");
            sb.Append("<p>Thank you for booking a session with <strong>" + mentorName + "</strong>.</p>");
            // Booking Details Table
            sb.Append("<h3>Your Booking Details:</h3>");
            sb.Append("<table style='width: 100%; border-collapse: collapse;'>");
            sb.Append("<tr style='background-color: #f2f2f2;'>");
            sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Mentor</th>");
            sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Date</th>");
            sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Time</th>");
            sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Status</th>");
            sb.Append("</tr>");
            foreach (var sessionTime in sessionTimes)
            {
                sb.Append("<tr>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>" + mentorName +
                          "</td>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>" +
                          sessionTime.FreeDay.ToString("yyyy-MM-dd") +
                          "</td>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>" +
                          sessionTime.StartTime.ToString("hh:mm tt") + " - " +
                          sessionTime.EndTime.ToString("hh:mm tt") + "</td>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>Pending</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            // Status Message
            sb.Append(
                "<p style='color: #FF6347;'>Your booking is currently awaiting confirmation from the mentor.</p>");
            sb.Append(
                "<p>Once the mentor confirms your session, you will receive another email with the confirmation details.</p>");

            // Footer
            sb.Append("<hr style='border-top: 1px solid #ddd;' />");
            sb.Append(
                "<p style='color: gray; font-size: 12px;'>Need help? Contact us at <a href='mailto:support@mentorbooking.com'>support@mentorbooking.com</a>.</p>");
            sb.Append(
                "<p style='color: gray; font-size: 12px;'>© 2024 Mentor Booking System. All rights reserved.</p>");
            sb.Append("</div>");
            return sb.ToString();
        }

        private string BuildMentorAcceptEmailBody(Guid sessionId, string studentName, string mentorName,
            List<Guid> scheduleAvailableId)
        {
            var sb = new StringBuilder();
            var sessionTimes = scheduleAvailableId.Select(scheduleId =>
                    _schedulesAvailableRepository.GetSchedulesAvailableAsync(scheduleId).GetAwaiter().GetResult()!)
                .ToList();
            var acceptLink =
                HtmlEncoder.Default.Encode(
                    $"https://localhost:7147/api/BookingMentor/accept-booking?SessionId={sessionId}&accept=true");
            var rejectLink = HtmlEncoder.Default.Encode(
                $"https://localhost:7147/api/BookingMentor/accept-booking?SessionId={sessionId}&accept=false");

            sb.Append("<div style='font-family: Arial, sans-serif; margin: 20px;'>");
            sb.Append("<h2 style='color: #4CAF50;'>Hey, " + mentorName + "!</h2>");
            sb.Append($"<p>You have one order booking from {studentName}. Please select one of the two options.</p>");
            sb.Append("<h3>His Booking Details:</h3>");
            sb.Append("<table style='width: 100%; border-collapse: collapse;'>");
            sb.Append("<tr style='background-color: #f2f2f2;'>");
            sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Mentor</th>");
            sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Date</th>");
            sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Time</th>");
            sb.Append("<th style='padding: 8px; border: 1px solid #ddd;'>Status</th>");
            sb.Append("</tr>");

            foreach (var sessionTime in sessionTimes)
            {
                sb.Append("<tr>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>" + mentorName +
                          "</td>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>" +
                          sessionTime.FreeDay.ToString("yyyy-MM-dd") + "</td>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>" +
                          sessionTime.StartTime.ToString("hh:mm tt") + " - " +
                          sessionTime.EndTime.ToString("hh:mm tt") + "</td>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>Pending</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            sb.Append("<h3>Select one of the options below:</h3>");
            sb.Append("<hr style='border-top: 1px solid #ddd;' />");

            // Container table to align buttons on the same row
            sb.Append(
                "<table role='presentation' cellspacing='0' cellpadding='0' border='0' style='width: 100%; text-align: center; margin-top: 20px;'>");
            sb.Append("<tr>");

            // Accept button
            sb.Append("<td style='padding: 10px;'>");
            sb.Append($"<a href='{acceptLink}' target='_blank' style='");
            sb.Append(
                "background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; font-size: 16px;");
            sb.Append("font-family: Arial, sans-serif; display: inline-block;'>Accept</a>");
            sb.Append("</td>");

            // Reject button
            sb.Append("<td style='padding: 10px;'>");
            sb.Append($"<a href='{rejectLink}' target='_blank' style='");
            sb.Append(
                "background-color: #FF0033; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; font-size: 16px;");
            sb.Append("font-family: Arial, sans-serif; display: inline-block;'>Reject</a>");
            sb.Append("</td>");

            sb.Append("</tr>");
            sb.Append("</table>");

            sb.Append(
                "<p style='color: gray; font-size: 12px;'>If you did not create this account, you can ignore this email.</p>");
            sb.Append(
                "<p style='color: gray; font-size: 12px;'>© 2024 Mentor Booking System. All rights reserved.</p>");
            sb.Append("</div>");

            return sb.ToString();
        }
    }
}