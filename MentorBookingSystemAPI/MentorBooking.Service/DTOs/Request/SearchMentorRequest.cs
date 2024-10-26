using MentorBooking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Request
{
    public class SearchMentorRequest
    {
        public string? SearchText { get; set; }

        public Mentor ToMentor()
        {
            Mentor mentor = new Mentor();
            mentor.User.FirstName = SearchText;
            mentor.User.LastName = SearchText;
            return mentor;
        }
    }
}
