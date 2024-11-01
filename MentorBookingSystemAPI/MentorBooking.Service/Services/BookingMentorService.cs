using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Services
{
    public class BookingMentorService : IBookingMentorService
    {
        private readonly IMentorSupportSessionRepository _sessionRepository;
        private readonly IMentorWorkScheduleRepository _workScheduleRepository;
        public BookingMentorService(IMentorSupportSessionRepository sessionRepository, IMentorWorkScheduleRepository workScheduleRepository)
        {
            _sessionRepository = sessionRepository;
            _workScheduleRepository = workScheduleRepository;
        }
        public async Task<ApiResponse> BookingMentor(MentorSupportSessionRequest request)
        {
            if(await _workScheduleRepository.CheckWordScheduleAsync(request.ScheduleAvailableId))
            {
                return new ApiResponse
                {
                    Status = "Error",
                    Message = "This Schedule is not availabe"
                };
            }
            var mentorSupportSession = new MentorSupportSession()
            {
                SessionId = Guid.NewGuid(),
                SessionCount = request.SessionCount,
                PointsPerSession = request.PointPerSession,
                TotalPoints = request.SessionCount * request.PointPerSession,
                MentorId=request.MentorId,
                //why noi guid
                //GroupId=Guid.NewGuid()
            };
            var mentorWorkSchedule = new MentorWorkSchedule()
            {
                ScheduleAvailableId = request.ScheduleAvailableId,
                SessionId = mentorSupportSession.SessionId,
                UnavailableDate = true,
                ScheduleId = request.ScheduleId,
            };
            if (await _sessionRepository.AddMentorSupportSessionAsync(mentorSupportSession) && await _workScheduleRepository.AddMentorWordScheduleAsync(mentorWorkSchedule))
            {
                return new ApiResponse
                {
                    Status = "Success",
                    Message = "Booking success",
                    Data = new MentorSupportSessionResponse
                    {
                        StudentId = request.StudentId,
                        SessionCount = request.SessionCount,
                        PointPerSession = request.PointPerSession,
                        TotalPoint = request.SessionCount * request.PointPerSession,
                        FreeDay = request.FreeDay,
                        StartTime = request.StartTime,
                        EndTime = request.EndTime,
                    }
                };
            }
            else return new ApiResponse
            {
                Status = "Error",
                Message = "Booking failed"
            };
        }
    }
}
