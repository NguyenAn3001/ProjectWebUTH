using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Services
{
    public class AcceptBookingSession : IAcceptBookingSession
    {
        private readonly IMentorSupportSessionRepository _mentorSupportSession;
        private readonly IMentorWorkScheduleRepository _workScheduleRepository;
        private readonly ISchedulesAvailableRepository _schedulesAvailableRepository;
        private readonly IUserPointRepository _userPointRepository;
        private readonly IStudentGroupRepository _studentGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISenderEmail _senderEmail;

        public AcceptBookingSession(IMentorSupportSessionRepository mentorSupportSession,
            IMentorWorkScheduleRepository workScheduleRepository,
            ISchedulesAvailableRepository schedulesAvailableRepository, IUserPointRepository userPointRepository, IStudentGroupRepository studentGroupRepository, IUserRepository userRepository,
            ISenderEmail senderEmail)
        {
            _mentorSupportSession = mentorSupportSession;
            _workScheduleRepository = workScheduleRepository;
            _userPointRepository = userPointRepository;
            _studentGroupRepository = studentGroupRepository;
            _schedulesAvailableRepository = schedulesAvailableRepository;
            _userRepository = userRepository;
            _senderEmail = senderEmail;
        }

        public async Task<ApiResponse> AcceptSession(Guid SessionId, bool acceptSession)
        {
            var accept = await _mentorSupportSession.GetMentorSupportSessionAsync(SessionId);
            var workSchedule = _workScheduleRepository.GetMentorWorkSchedule(SessionId);
            if (accept == null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Session is not found"
                };
            }

            var availableSchedulesId = workSchedule.Select(x => x.ScheduleAvailableId).ToList();
            var userId = accept.MentorId.ToString();
            var studentId = accept.Group.CreateBy.ToString();
            var student = await _userRepository.FindByIdAsync(studentId);
            var mentor = await _userRepository.FindByIdAsync(userId);
            accept.SessionConfirm = acceptSession;
            if (acceptSession)
            {
                foreach (var item in workSchedule)
                {
                    item.UnavailableDate = true;
                    await _workScheduleRepository.UpdateMentorWorkSchedule(item);
                }

                var checkUpdate = await _mentorSupportSession.UpdateMentorSupportSessionAsync(accept);
                if (!checkUpdate)
                {
                    return new ApiResponse()
                    {
                        Status = "Error",
                        Message = "Something wrong",
                    };
                }
                else
                {
                    var students = _studentGroupRepository.GetAllStudentInGroup(accept.GroupId);
                    if (students == null)
                    {
                        return new ApiResponse()
                        {
                            Status = "Error",
                            Message = "Error Payment",
                        };
                    }
                    foreach (var item in students)
                    {
                        var pointPayment = accept.TotalPoints;
                        await _userPointRepository.SetUserPoint(item.StudentId, pointPayment);
                    }
                    await _userPointRepository.SetUserPoint(accept.MentorId, accept.TotalPoints * students.Count());
                }
                await _senderEmail.SendEmailAsync(student?.Email!,
                    $"Booking Confirmed: Your Session with {mentor?.FirstName} {mentor?.LastName} is Accepted!",
                    BuildStudentBookingAcceptedEmailBody(availableSchedulesId,
                        $"{mentor?.FirstName} {mentor?.LastName}"), isBodyHtml: true);
                return new ApiResponse()
                {
                    Status = "Success",
                    Message = "Accept session",
                    Data = new MentorSupportSessionResponse()
                    {
                        SessionId = accept.SessionId,
                        MentorId = accept.MentorId,
                        GroupId = accept.GroupId,
                        SessionCount = accept.SessionCount,
                        PointPerSession = accept.PointsPerSession,
                        TotalPoint = accept.TotalPoints,
                        SessionConfirm = accept.SessionConfirm,
                    }
                };
            }

            var deleteWorkSchedule = await _workScheduleRepository.DeleteMentorWorkScheduleAsync(SessionId);
            var deleteSession = await _mentorSupportSession.DeleteMentorSupportSessionAsync(SessionId);
            if (deleteSession && deleteWorkSchedule)
            {
                //response mail to student
                await _senderEmail.SendEmailAsync(student?.Email!,
                    $"Booking Rejected: Your Session with {mentor?.FirstName} {mentor?.LastName} is Rejected!",
                    BuildStudentBookingRejectedEmailBody(availableSchedulesId,
                        $"{mentor?.FirstName} {mentor?.LastName}"), isBodyHtml: true);
                return new ApiResponse()
                {
                    Status = "Success",
                    Message = "Unaccept session success",
                    Data = new MentorSupportSessionResponse()
                    {
                        SessionId = accept.SessionId,
                        MentorId = accept.MentorId,
                        GroupId = accept.GroupId,
                        SessionCount = accept.SessionCount,
                        PointPerSession = accept.PointsPerSession,
                        TotalPoint = accept.TotalPoints,
                        SessionConfirm = accept.SessionConfirm,
                    }
                };
            }

            return new ApiResponse()
            {
                Status = "Error",
                Message = "Unaccept session fail",
                Data = new MentorSupportSessionResponse()
                {
                    SessionId= accept.SessionId,
                    MentorId= accept.MentorId,
                    GroupId = accept.GroupId,
                    SessionCount = accept.SessionCount,
                    PointPerSession = accept.PointsPerSession,
                    TotalPoint = accept.TotalPoints,
                    SessionConfirm = accept.SessionConfirm,
                }
            };
        }
        public List<ApiResponse> GetAllSessionUnAccept(Guid MentorId)
        {
            var listApiResponse = new List<ApiResponse>();
            var listSession = _mentorSupportSession.GetAllMentorSupportSessionAsync(MentorId);
            var errorApiResponse = new ApiResponse()
            {
                Status = "Success",
                Message = "No unaccept session"
            };
            if (listSession == null)
            {
                listApiResponse.Add(errorApiResponse);
                return listApiResponse;
            }

            foreach (var item in listSession)
            {
                if (!item.SessionConfirm)
                {
                    var session = new ApiResponse()
                    {
                        Status = "Success",
                        Message = "Session is found",
                        Data = new MentorSupportSessionResponse()
                        {
                            SessionId = item.SessionId,
                            MentorId = item.MentorId,
                            GroupId = item.GroupId,
                            SessionCount = item.SessionCount,
                            PointPerSession = item.PointsPerSession,
                            TotalPoint = item.TotalPoints,
                            SessionConfirm=item.SessionConfirm,
                        }
                    };
                    listApiResponse.Add(session);
                }
            }

            if (listApiResponse == null)
            {
                listApiResponse.Add(errorApiResponse);
                return listApiResponse;
            }

            return listApiResponse;
        }

        private string BuildStudentBookingAcceptedEmailBody(List<Guid> scheduleAvailableId, string mentorName)
        {
            var sessionTimes = scheduleAvailableId.Select(scheduleId =>
                    _schedulesAvailableRepository.GetSchedulesAvailableAsync(scheduleId).GetAwaiter().GetResult()!)
                .ToList();
            var sb = new StringBuilder();
            // General Header and Welcome Message
            sb.Append("<div style='font-family: Arial, sans-serif; margin: 20px;'>");
            sb.Append("<h2 style='color: #4CAF50;'>Your booking has been accepted by <strong>" + mentorName +
                      "</strong></h2>");
            sb.Append("<p>Congratulations!  <strong>" + mentorName + "</strong> has confirmed your booking.</p>");
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
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>Confirmed</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            // Status Message
            sb.Append(
                "<p style='color: #4CAF50;'>Your booking is confirmed. You can now prepare for your session.</p>");
            sb.Append("<p>If you have any questions or need to make changes, please contact us.</p>");

            // Footer
            sb.Append("<hr style='border-top: 1px solid #ddd;' />");
            sb.Append(
                "<p style='color: gray; font-size: 12px;'>Need help? Contact us at <a href='mailto:support@mentorbooking.com'>support@mentorbooking.com</a>.</p>");
            sb.Append(
                "<p style='color: gray; font-size: 12px;'>© 2024 Mentor Booking System. All rights reserved.</p>");
            sb.Append("</div>");

            return sb.ToString();
        }

        private string BuildStudentBookingRejectedEmailBody(List<Guid> scheduleAvailableId, string mentorName)
        {
            var sessionTimes = scheduleAvailableId.Select(scheduleId =>
                    _schedulesAvailableRepository.GetSchedulesAvailableAsync(scheduleId).GetAwaiter().GetResult()!)
                .ToList();
            var sb = new StringBuilder();

            // General Header and Message for Rejection
            sb.Append("<div style='font-family: Arial, sans-serif; margin: 20px;'>");
            sb.Append("<h2 style='color: #FF0033;'>Unfortunately, your booking was rejected by <strong>" + mentorName +
                      "</strong>.</h2>");
            sb.Append("<p>We apologize, but <strong>" + mentorName +
                      "</strong> was unable to confirm your booking request at this time.</p>");

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
                          sessionTime.FreeDay.ToString("yyyy-MM-dd") + "</td>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>" +
                          sessionTime.StartTime.ToString("hh:mm tt") + " - " +
                          sessionTime.EndTime.ToString("hh:mm tt") + "</td>");
                sb.Append("<td style='padding: 8px; border: 1px solid #ddd; text-align: center;'>Rejected</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            // Status Message
            sb.Append(
                "<p style='color: #FF6347;'>You may try booking another session with a different mentor or at another time.</p>");
            sb.Append("<p>If you need assistance or have any questions, feel free to reach out to us.</p>");

            // Footer
            sb.Append("<hr style='border-top: 1px solid #ddd;' />");
            sb.Append(
                "<p style='color: gray; font-size: 12px;'>Need help? Contact us at <a href='mailto:support@mentorbooking.com'>support@mentorbooking.com</a>.</p>");
            sb.Append(
                "<p style='color: gray; font-size: 12px;'>© 2024 Mentor Booking System. All rights reserved.</p>");
            sb.Append("</div>");

            return sb.ToString();
        }
    }
}