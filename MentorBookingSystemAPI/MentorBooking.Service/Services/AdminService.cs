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
    public class AdminService : IAdminService
    {
        private readonly IMentorSupportSessionRepository _sessionRepository;
        private readonly IUserPointRepository _userPointRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMentorRepository _mentorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMentorWorkScheduleRepository _workScheduleRepository;
        public AdminService(IMentorSupportSessionRepository sessionRepository, IUserPointRepository userPointRepository, IUserRepository userRepository
            , IMentorRepository mentorRepository, IStudentRepository studentRepository,IMentorWorkScheduleRepository mentorWorkScheduleRepository)
        {
            _sessionRepository = sessionRepository;
            _userPointRepository = userPointRepository;
            _userRepository = userRepository;
            _mentorRepository = mentorRepository;
            _studentRepository = studentRepository;
            _workScheduleRepository = mentorWorkScheduleRepository;
        }

        public async Task<ApiResponse> DeletePointTransaction(Guid PointTransactionId)
        {
            var deletePointTransaction = await _userPointRepository.DeletePointTransaction(PointTransactionId);
            if(!deletePointTransaction)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "delete point transaction fail"
                };
            }
            return new ApiResponse()
            {
                Status = "Success",
                Message = "delete point transaction success"
            };
        }

        public async Task<ApiResponse> DeleteSession(Guid sessionId)
        {
           var deleteWorkSchedule= await _workScheduleRepository.DeleteMentorWorkScheduleAsync(sessionId);
           var deleteSession = await _sessionRepository.DeleteMentorSupportSessionAsync(sessionId);
           if(!deleteSession || !deleteWorkSchedule) 
           {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "delete session fail"
                };
           }
            return new ApiResponse()
            {
                Status = "Success",
                Message = "delete session success"
            };
        }
        public async Task<List<ApiResponse>> GetAllMentor()
        {
            var allUsers = _userRepository.GetAllUser();
            var listApiResponse = new List<ApiResponse>();
            foreach (var item in allUsers)
            {
                var aMentor=await _mentorRepository.GetMentorByIdAsync(item.Id);
                if(aMentor != null)
                {
                    ApiResponse response = new ApiResponse()
                    {
                        Status = "Succes",
                        Message = "Found",
                        Data = new MentorUserResponse()
                        {
                            MentorId = aMentor.UserId,
                            UserName = item.UserName,
                            CreateAt = aMentor.CreateAt
                        }
                    };
                    listApiResponse.Add(response);
                }    
            }
            return listApiResponse;
        }

        public List<ApiResponse> GetAllPointTransactions()
        {
            var allPointTransaction = _userPointRepository.GetAllPointTransaction();
            var listApiResponse = new List<ApiResponse>();
            foreach (var item in allPointTransaction)
            {
                ApiResponse response = new ApiResponse()
                {
                    Status = "Succes",
                    Message = "Found",
                    Data = item
                };
                listApiResponse.Add(response);
            }
            return listApiResponse;
        }

        public List<ApiResponse> GetAllSessions()
        {
            var allSession = _sessionRepository.GetALlSession();
            var listApiResponse = new List<ApiResponse>();
            foreach (var item in allSession)
            {
               if(item.SessionConfirm)
               {
                    ApiResponse response = new ApiResponse()
                    {
                        Status = "Succes",
                        Message = "Found",
                        Data = new MentorSupportSessionResponse()
                        {
                            SessionId = item.SessionId,
                            MentorId = item.MentorId,
                            GroupId = item.GroupId,
                            SessionCount = item.SessionCount,
                            TotalPoint = item.TotalPoints
                        }
                    };
                    listApiResponse.Add(response);
                }    
            }
            return listApiResponse;
        }

        public async Task<List<ApiResponse>> GetAllStudent()
        {
            var allUsers = _userRepository.GetAllUser();
            var listApiResponse = new List<ApiResponse>();
            foreach (var item in allUsers)
            {
                var aStudent = await _studentRepository.GetStudentByIdAsync(item.Id);
                if(aStudent != null)
                {
                    ApiResponse response = new ApiResponse()
                    {
                        Status = "Succes",
                        Message = "Found",
                        Data = new MentorUserResponse()
                        {
                            MentorId = aStudent.UserId,
                            UserName = item.UserName,
                            CreateAt = aStudent.CreateAt
                        }
                    };
                    listApiResponse.Add(response);
                }
            }
            return listApiResponse;
        }
    }
}
