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

        public Users ToUsers()
        {
            return new Users() { FirstName = SearchText, LastName = SearchText };
        }
    }
}
