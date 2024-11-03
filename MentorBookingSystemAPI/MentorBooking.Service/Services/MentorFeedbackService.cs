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
        public MentorFeedbackService(IMentorFeedbackRepository mentorFeedbackRepository)
        {
            _mentorFeedbackRepository = mentorFeedbackRepository;
        }
        public async Task<ApiResponse> AddStudentCommentAsync(StudentCommentRequest studentComment)
        {
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
                        StudentId = mentorFeedback.StudentId,
                        Comment = mentorFeedback.Comment,
                        Rating = mentorFeedback.Rating,
                        CreateAt = mentorFeedback.CreateAt,
                        FirstName = studentComment.FirstName,
                        LastName = studentComment.LastName,
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

        public List<ApiResponse> GetAllMentorFeedback(Guid MentorId)
        {
            throw new NotImplementedException();
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
            };
        }

        public async Task<bool> UpdateMentorFeedbackAsync(MentorFeedback oldMentorFeedback, StudentCommentRequest studentComment)
        {
            oldMentorFeedback.FeedbackId = Guid.NewGuid();
            oldMentorFeedback.Comment = studentComment.Comment;
            oldMentorFeedback.Rating= studentComment.Rating;
            oldMentorFeedback.CreateAt= DateTime.Now;
            if (await _mentorFeedbackRepository.AddMentorFeedbackAsync(oldMentorFeedback))
            {
                return true;
            }
            return false;
            
        }
    }
}
