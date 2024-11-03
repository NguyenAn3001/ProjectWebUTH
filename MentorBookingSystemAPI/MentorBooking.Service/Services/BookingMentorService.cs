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
            if(request.dateBookings.Count()>request.SessionCount)
            {
                return new ApiResponse
                {
                    Status = "Error",
                    Message = "Numbers of session cant not over Session count"
                };
            }    
            foreach(var unavailable in request.dateBookings)
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
                    Message = "This Schedule is not availabe"
                };
            }
            foreach (var dateBooking in request.dateBookings)
            {
                var mentorWorkSchedule = new MentorWorkSchedule()
                {
                    SessionId = mentorSupportSession.SessionId,
                    UnavailableDate = true,
                    ScheduleId = Guid.NewGuid(),
                    ScheduleAvailableId=dateBooking
                };
                if(!await _workScheduleRepository.AddMentorWordScheduleAsync(mentorWorkSchedule))
                {
                    return new ApiResponse
                    {
                        Status = "Error",
                        Message = "This Schedule is not availabe"
                    };
                }    
            }
            return new ApiResponse()
            {
                Status="Success",
                Message="Booking is success",
                Data= new MentorSupportSessionResponse()
                {
                    StudentId=request.StudentId,
                    GroupId=request.GroupId,
                    SessionCount=request.SessionCount,
                    PointPerSession=request.PointPerSession,
                    TotalPoint=request.SessionCount * request.PointPerSession,
                }
            };
        }

        public async Task<ApiResponse> DeleteMentorSupportSessionAsync(Guid SessionId)
        {
            var existMentorSession = await _sessionRepository.GetMentorSupportSessionAsync(SessionId);
            var existWorkSchedule = _workScheduleRepository.GetMentorWorkSchedule(SessionId).ToList();
            if(existMentorSession==null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Mentor support session is not found"
                };
            }
            if(existWorkSchedule.Count()==0)
            {
                return new ApiResponse()
                { 
                    Status="Error",
                    Message="Work Schedule session is not found"
                };
            }
            var deleteWorkSChedule = await _workScheduleRepository.DeleteMentorWorkScheduleAsync(SessionId);
            var deleteMentorSession = await _sessionRepository.DeleteMentorSupportSessionAsync(SessionId);
            if(!deleteMentorSession)
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
                    Message="Work Schedule session is deleted or missing"
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
            throw new NotImplementedException();
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
                Message = "Mentor support session is found"
            };
        }
    }
}
