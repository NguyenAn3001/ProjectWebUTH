using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class StudentProfilesResponse
    {
        public Guid StudentId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
