using MentorBooking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class MentorSearchingResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public List<string>? SkillName { get; set; }
    }
    public static class MentorExtensions
    {
        public static MentorSearchingResponse ToMentorSearchingResponse(this Mentor mentor)
        {
            return new MentorSearchingResponse()
            {
                FirstName = mentor.User.FirstName,
                LastName = mentor.User.LastName,
                Image = mentor.User.Image
            };
        }
    }
}
