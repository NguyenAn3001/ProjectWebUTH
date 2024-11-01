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
        public async Task<ApiResponse> AddMentorFeedbackAsync(Users user, Guid mentorId, Guid sessionId, StudentCommentRequest studentComment)
        {
            var mentorFeedback = new MentorFeedback()
            {
                FeedbackId = Guid.NewGuid(),
                SessionId= sessionId,
                StudentId = user.Id,
                MentorId = mentorId,
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
                        StudentId = user.Id,
                        Comment = mentorFeedback.Comment,
                        Rating = mentorFeedback.Rating,
                        CreateAt = mentorFeedback.CreateAt,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    }
                };
            }
            return new ApiResponse()
            {
                Status = "Error",
                Message = "New comment hasn't been created",
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
