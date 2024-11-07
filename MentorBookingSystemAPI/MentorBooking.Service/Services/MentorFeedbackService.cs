using MentorBooking.Repository.Data;
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
    public class MentorFeedbackService : IMentorFeedbackService
    {
        private readonly IMentorFeedbackRepository _mentorFeedbackRepository;
        private readonly IMentorSupportSessionRepository _mentorSupportSessionRepository;
        private readonly IUserRepository _userRepository;
        public MentorFeedbackService(IMentorFeedbackRepository mentorFeedbackRepository,IUserRepository userRepository, IMentorSupportSessionRepository mentorSupportSessionRepository)
        {
            _mentorFeedbackRepository = mentorFeedbackRepository;
            _userRepository = userRepository;
            _mentorSupportSessionRepository = mentorSupportSessionRepository;
        }
        public async Task<ApiResponse> AddStudentCommentAsync(StudentCommentRequest studentComment)
        {
            if(!await _mentorSupportSessionRepository.CheckMentorSessionAsync(studentComment.SessionId))
            {
                return new ApiResponse()
                {
                    Status="Error",
                    Message="Your session is unaccepted by mentor"
                };
            }    
            var mentorFeedback = new MentorFeedback()
            {
                FeedbackId = Guid.NewGuid(),
                SessionId= studentComment.SessionId,
                StudentId =studentComment.UserId,
                MentorId = studentComment.MentorId,
                Rating = studentComment.Rating,
                Comment = studentComment.Comment,
                CreateAt = DateTime.Now,
            };
            if (await _mentorFeedbackRepository.AddMentorFeedbackAsync(mentorFeedback))
            {
                return new ApiResponse()
                {
                    Status = "Success",
                    Message = "New comment has been created",
                    Data = new StudentCommentResponse()
                    {
                        FeedbackId=mentorFeedback.FeedbackId,
                        SessionId=mentorFeedback.SessionId,
                        MentorId=mentorFeedback.MentorId,
                        StudentId = mentorFeedback.StudentId,
                        Comment = mentorFeedback.Comment,
                        Rating = mentorFeedback.Rating,
                        CreateAt = mentorFeedback.CreateAt,
                    }
                };
            }
            return new ApiResponse()
            {
                Status = "Error",
                Message = "New comment hasn't been created",
            };
        }

        public async Task<ApiResponse> DeleteMentorFeedbackAsync(Guid MentorFeedbackId)
        {
            var existMentorFeedback = await _mentorFeedbackRepository.GetMentorFeedbackAsync(MentorFeedbackId);
            if(existMentorFeedback==null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Not Found"
                };
            }
            var deleteMentorFeedback = await _mentorFeedbackRepository.DeleteMentorFeedbackAsync(MentorFeedbackId);
            if(!deleteMentorFeedback)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Mentor Feedback is missing or deleted"
                };
            }
            return new ApiResponse()
            {
                Status = "Success",
                Message = "Mentor Feedback is deleted"
            };
        }

        public async Task<List<ApiResponse>> GetAllMentorFeedback(Guid MentorId)
        {
            List<ApiResponse> ListMentorFeedback = new List<ApiResponse>();
            var existMentorFeedback = _mentorFeedbackRepository.GetAllMentorFeedbacksAsync(MentorId);
            var response = new ApiResponse()
            {
                Status = "Error",
                Message = "Not Found"
            };
            if (existMentorFeedback==null)
            {
                ListMentorFeedback.Add(response);
                return ListMentorFeedback;
            }
            foreach(var item in existMentorFeedback)
            {
                var student =await _userRepository.FindByIdAsync(item.StudentId.ToString());
                if(student==null)
                {
                    ListMentorFeedback.Add(response);
                    return ListMentorFeedback;
                }
                var StudentComment = new ApiResponse()
                {
                    Status = "Success",
                    Message = "It is ok",
                    Data= new StudentCommentResponse()
                    {
                        FeedbackId=item.FeedbackId,
                        SessionId = item.SessionId,
                        MentorId = item.StudentId,
                        StudentId = item.StudentId,
                        Rating = item.Rating,
                        Comment = item.Comment,
                        CreateAt = item.CreateAt,
                    }
                };
                ListMentorFeedback.Add(StudentComment);
            }    
            return ListMentorFeedback;
        }

        public async Task<ApiResponse> GetMentorFeedback(Guid MentorFeedbackId)
        {
            var existMentorFeedback = await _mentorFeedbackRepository.GetMentorFeedbackAsync(MentorFeedbackId);
            if (existMentorFeedback == null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Not Found"
                };
            }
            return new ApiResponse()
            {
                Status = "Success",
                Message = "Mentor Feedback is found",
                Data = new StudentCommentResponse()
                {
                    FeedbackId = existMentorFeedback.StudentId,
                    SessionId=existMentorFeedback.SessionId,
                    MentorId=existMentorFeedback.StudentId,
                    StudentId = existMentorFeedback.StudentId,
                    Rating=existMentorFeedback.Rating,
                    Comment=existMentorFeedback.Comment,
                    CreateAt=existMentorFeedback.CreateAt,

                }
            };
        }
        public async Task<ApiResponse> UpdateMentorFeedbackAsync(StudentCommentRequest studentComment)
        {
            var mentorUpdateFeedback = new MentorFeedback()
            {
                FeedbackId = Guid.NewGuid(),
                SessionId = studentComment.SessionId,
                StudentId = studentComment.UserId,
                MentorId = studentComment.MentorId,
                Rating = studentComment.Rating,
                Comment = studentComment.Comment,
                CreateAt = DateTime.Now,
            };
            if (await _mentorFeedbackRepository.UpdateMentorFeedbackAsync(mentorUpdateFeedback))
            {
                return new ApiResponse
                { 
                    Status="Success",
                    Message="Feedback is updated"
                };
            }
            return new ApiResponse
            {
                Status = "Error",
                Message = "Feedback is not updated, please try agian"
            };
            
        }
    }
}
