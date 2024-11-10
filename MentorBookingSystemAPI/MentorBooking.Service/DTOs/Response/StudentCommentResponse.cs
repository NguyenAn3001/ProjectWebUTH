using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class StudentCommentResponse
    {
        public Guid FeedbackId { get; set; }
        public Guid SessionId { get; set; }
        public Guid StudentId { get; set; }
        public Guid MentorId { get; set; }
        public string StudentName { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
