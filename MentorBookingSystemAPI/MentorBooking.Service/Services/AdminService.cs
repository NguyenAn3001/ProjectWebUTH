using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private readonly IStudentGroupRepository _studentGroupRepository;
        private readonly IMentorFeedbackRepository _feedbackRepository;
        private readonly IMentorSkillRepository _mentorSkillRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IRoleRepository _roleRepository;
        public AdminService(IMentorSupportSessionRepository sessionRepository, IUserPointRepository userPointRepository, IUserRepository userRepository
            , IMentorRepository mentorRepository, IStudentRepository studentRepository,IMentorWorkScheduleRepository mentorWorkScheduleRepository,IStudentGroupRepository studentGroupRepository
            ,IMentorFeedbackRepository mentorFeedbackRepository,ISkillRepository skillRepository,IMentorSkillRepository mentorSkillRepository,IRoleRepository roleRepository)
        {
            _sessionRepository = sessionRepository;
            _userPointRepository = userPointRepository;
            _userRepository = userRepository;
            _mentorRepository = mentorRepository;
            _studentRepository = studentRepository;
            _workScheduleRepository = mentorWorkScheduleRepository;
            _studentGroupRepository = studentGroupRepository;
            _feedbackRepository = mentorFeedbackRepository;
            _skillRepository = skillRepository;
            _mentorSkillRepository= mentorSkillRepository;
            _roleRepository= roleRepository;
        }

        public async Task<List<ApiResponse>> AllFeedback()
        {
            var allUsers = _userRepository.GetAllUser();
            var listApiResponse = new List<ApiResponse>();
            foreach(var item in allUsers)
            {
                var aMentorFeedback = _feedbackRepository.GetAllMentorFeedbacksAsync(item.Id);
                if(aMentorFeedback != null)
                {
                    foreach(var feedback in  aMentorFeedback)
                    {
                        var aStudent = await _userRepository.FindByIdAsync(feedback.StudentId.ToString());
                        ApiResponse response = new ApiResponse()
                        {
                            Status = "Success",
                            Message="Found",
                            Data=new StudentCommentResponse()
                            {
                                MentorId=item.Id,
                                FeedbackId=feedback.FeedbackId,
                                SessionId=feedback.SessionId,
                                StudentId=feedback.StudentId,
                                StudentName=aStudent.FirstName +" "+ aStudent.LastName,
                                Comment=feedback.Comment,
                                Rating=feedback.Rating,
                                CreateAt=feedback.CreateAt,
                            }
                        };
                        listApiResponse.Add(response);
                    }    
                }    
            } 
            return listApiResponse;
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
                var allSession = _sessionRepository.GetAllMentorSupportSessionAsync(item.Id);
                var allFeedback= _feedbackRepository.GetAllMentorFeedbacksAsync(item.Id);
                var allSkill = _mentorSkillRepository.GetMentorSkillByIdAsync(item.Id);
                var allMentorSkill = new List<string>();
                foreach(var skill in allSkill)
                {
                    var askill = await _skillRepository.GetSkillByIdAsync(skill.SkillId);
                    allMentorSkill.Add(askill.Name);
                }    
                var rating = 0;
                var sessionCount = 0;
                if (allFeedback != null)
                {
                    foreach (var rate in allFeedback)
                    {
                        rating = rating + rate.Rating;
                    }
                    rating=rating/allFeedback.Count();
                    sessionCount= allFeedback.Count();
                }    
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
                            CountSession=sessionCount,
                            Ratings = rating,
                            Skills=allMentorSkill,
                            CreateAt = aMentor.CreateAt
                        }
                    };
                    listApiResponse.Add(response);
                }    
            }
            return listApiResponse;
        }

        public async Task<List<ApiResponse>?> GetAllPointTransactionsDecrease()
        {
            var allPointTransaction = _userPointRepository.GetAllPointTransaction();
            var listApiResponse = new List<ApiResponse>();
            foreach (var item in allPointTransaction)
            {
                if (!item.TransactionType)
                {
                    var aUser = await _userRepository.FindByIdAsync(item.UserId.ToString());
                    if (aUser != null)
                    {
                        var aRole = await _roleRepository.GetRoleOfUser(aUser);

                        ApiResponse response = new ApiResponse()
                        {
                            Status = "Succes",
                            Message = "Found",
                            Data = new TransactionResponse()
                            {
                                TransactionId = item.TransactionId,
                                PointsChanged = item.PointsChanged,
                                TransactionType = item.TransactionType,
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                UserId = item.UserId,
                                Name = aUser.FirstName + " " + aUser.LastName,
                                Email = aUser.Email,
                                Role = aRole
                            }
                        };
                        listApiResponse.Add(response);
                    }
                }
            }
            return listApiResponse;
        }

        public async Task<List<ApiResponse>?> GetAllPointTransactionsIncrease()
        {
            var allPointTransaction = _userPointRepository.GetAllPointTransaction();
            var listApiResponse = new List<ApiResponse>();
            foreach (var item in allPointTransaction)
            {
                if(item.TransactionType)
                {
                   var aUser = await _userRepository.FindByIdAsync(item.UserId.ToString());
                   if(aUser != null)
                   {
                        var aRole = await _roleRepository.GetRoleOfUser(aUser);
                        ApiResponse response = new ApiResponse()
                        {
                            Status = "Succes",
                            Message = "Found",
                            Data = new TransactionResponse()
                            {
                                TransactionId = item.TransactionId,
                                PointsChanged = item.PointsChanged,
                                TransactionType = item.TransactionType,
                                Description = item.Description,
                                CreateAt = item.CreateAt,
                                UserId = item.UserId,
                                Name = aUser.FirstName + " " + aUser.LastName,
                                Email = aUser.Email,
                                Role = aRole

                            }
                        };
                        listApiResponse.Add(response);
                    }    
                }    
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
                var aStudentPoint = await _userPointRepository.GetUserPoint(item.Id);
                var AcountGroup = _studentGroupRepository.GetListStudentInGroup(item.Id);
                if(aStudent != null)
                {
                    ApiResponse response = new ApiResponse()
                    {
                        Status = "Succes",
                        Message = "Found",
                        Data = new StudentUserResponse()
                        {
                            StudentId = aStudent.UserId,
                            UserName = item.UserName,
                            Email = item.Email,
                            PointBalance = aStudentPoint.PointsBalance,
                            countGroup = AcountGroup.Count(),
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
