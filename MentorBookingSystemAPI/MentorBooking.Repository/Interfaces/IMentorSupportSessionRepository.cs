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
    }
}
