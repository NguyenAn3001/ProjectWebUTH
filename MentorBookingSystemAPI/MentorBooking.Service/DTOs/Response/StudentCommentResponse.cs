using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class StudentCommentResponse
    {
        public Guid 
        public Guid StudentId { get; set; }
        public Guid MentorId { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreateAt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
