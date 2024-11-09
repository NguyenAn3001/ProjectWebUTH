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
        private readonly IUserPointRepository _userPointRepository;
        private readonly IStudentGroupRepository _studentGroupRepository;
        public AcceptBookingSession(IMentorSupportSessionRepository mentorSupportSession, IMentorWorkScheduleRepository workScheduleRepository, IUserPointRepository userPointRepository, IStudentGroupRepository studentGroupRepository)
        {
            _mentorSupportSession = mentorSupportSession;
            _workScheduleRepository = workScheduleRepository;
            _userPointRepository = userPointRepository;
            _studentGroupRepository = studentGroupRepository;
        }
        public async Task<ApiResponse> AcceptSession(Guid SessionId,bool acceptSession)
        {
            var accept= await _mentorSupportSession.GetMentorSupportSessionAsync(SessionId);
            var workSchedule=_workScheduleRepository.GetMentorWorkSchedule(SessionId);
            if(accept==null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Session is not found"
                };
            }
            if (accept.SessionConfirm)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "This session was accepted"
                };
            }
            accept.SessionConfirm= acceptSession;
            if(acceptSession)
            {
                foreach (var item in workSchedule)
                {
                    item.UnavailableDate = true;
                    await _workScheduleRepository.UpdateMentorWorkSchedule(item);
                }
                var checkUpdate=await _mentorSupportSession.UpdateMentorSupportSessionAsync(accept);
                if (!checkUpdate)
                {
                    return new ApiResponse()
                    {
                        Status = "Error",
                        Message = "Update work schedule wrong",
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
                        var studentPoint = await _userPointRepository.GetUserPoint(item.StudentId);
                        var studentPayment =studentPoint.PointsBalance - accept.TotalPoints;
                        await _userPointRepository.SetUserPoint(item.StudentId, (int)studentPayment);
                    }
                    var mentorPoint = await _userPointRepository.GetUserPoint(accept.MentorId);
                    var mentorPayment = mentorPoint.PointsBalance + accept.TotalPoints*students.Count();
                    await _userPointRepository.SetUserPoint(accept.MentorId, (int)mentorPayment );
                }
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
            var deleteWorkSchedule=await _workScheduleRepository.DeleteMentorWorkScheduleAsync(SessionId);
            var deleteSession = await _mentorSupportSession.DeleteMentorSupportSessionAsync(SessionId);
            if(deleteSession && deleteWorkSchedule)
            {
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
            var listApiResponse=new List<ApiResponse>();
            var listSession = _mentorSupportSession.GetAllMentorSupportSessionAsync(MentorId);
            var errorApiResponse = new ApiResponse()
            {
                Status = "Success",
                Message = "No unaccept session"
            };
            if(listSession==null)
            {
                listApiResponse.Add(errorApiResponse);
                return listApiResponse;
            }    
            foreach( var item in listSession )
            {
                if(!item.SessionConfirm)
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
            return listApiResponse;
        }
    }
}
