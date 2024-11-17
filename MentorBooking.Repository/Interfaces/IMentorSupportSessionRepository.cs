using MentorBooking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Interfaces
{
    public interface IMentorSupportSessionRepository
    {
        Task<bool> AddMentorSupportSessionAsync(MentorSupportSession mentorSupportSession);
        Task<MentorSupportSession?> GetMentorSupportSessionAsync(Guid SessionId);
        Task<MentorSupportSession?> GetMentorSupportSessionByGroupIdAsync(Guid GroupId);
        Task<bool> DeleteMentorSupportSessionAsync(Guid SessionId);
        List<MentorSupportSession>? GetAllMentorSupportSessionAsync(Guid MentorId);
        Task<bool> UpdateMentorSupportSessionAsync(MentorSupportSession mentorSupportSession);
        Task<bool> CheckMentorSessionAsync (Guid SessionId);
        List<MentorSupportSession> GetALlSession();
    }
}
