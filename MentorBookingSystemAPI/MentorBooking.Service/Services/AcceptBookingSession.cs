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
        public AcceptBookingSession(IMentorSupportSessionRepository mentorSupportSession, IMentorWorkScheduleRepository workScheduleRepository)
        {
            _mentorSupportSession = mentorSupportSession;
            _workScheduleRepository = workScheduleRepository;
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
                        Message = "Something wrong",
                    };
                }   
                return new ApiResponse()
                {
                    Status = "Success",
                    Message = "Accept session",
                    Data = new MentorSupportSessionResponse()
                    {
                        GroupId = accept.GroupId,
                        SessionCount = accept.SessionCount,
                        PointPerSession = accept.PointsPerSession,
                        TotalPoint = accept.TotalPoints
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
                        GroupId = accept.GroupId,
                        SessionCount = accept.SessionCount,
                        PointPerSession = accept.PointsPerSession,
                        TotalPoint = accept.TotalPoints
                    }
                };
            }
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Unaccept session fail",
                Data = new MentorSupportSessionResponse()
                {
                    GroupId = accept.GroupId,
                    SessionCount = accept.SessionCount,
                    PointPerSession = accept.PointsPerSession,
                    TotalPoint = accept.TotalPoints
                }
            };
        }

        public async Task<List<ApiResponse>> GetAllSessionUnAccept(Guid MentorId)
        {
            var listApiResponse=new List<ApiResponse>();
            var listSession = _mentorSupportSession.GetAllMentorSupportSessionAsync(MentorId);
            var errorApiResponse = new ApiResponse()
            {
                Status = "Error",
                Message = "Session is not Found"
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
                            GroupId = item.GroupId,
                            SessionCount = item.SessionCount,
                            PointPerSession = item.PointsPerSession,
                            TotalPoint = item.TotalPoints
                        }
                    };
                    listApiResponse.Add(session);
                }    
            }
            if(listApiResponse==null)
            {
                listApiResponse.Add(errorApiResponse);
                return listApiResponse;
            }    
            return listApiResponse;
        }
    }
}
