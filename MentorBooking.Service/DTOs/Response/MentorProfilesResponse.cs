using MentorBooking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class MentorProfilesResponse
    {
        public Guid MentorId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public byte? ExperienceYears { get; set; }
        public string? MentorDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<string>? Skills { get; set; }=new List<string>();
    }
}
