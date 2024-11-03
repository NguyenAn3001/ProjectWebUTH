using MentorBooking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Interfaces
{
    public interface IMentorFeedbackRepository
    {
        Task<bool> AddMentorFeedbackAsync(MentorFeedback mentorFeedback);
        Task<bool> DeleteMentorFeedbackAsync(Guid FeedbackId);
        Task<MentorFeedback?> GetMentorFeedbackAsync(Guid FeedbackId);
        List<MentorFeedback>? GetAllMentorFeedbacksAsync(Guid MentorId);
    }
}
