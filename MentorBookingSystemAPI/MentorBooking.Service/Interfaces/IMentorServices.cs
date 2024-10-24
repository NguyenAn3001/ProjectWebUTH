using MentorBooking.Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Interfaces
{
    public interface IMentorServices
    {
        public List<MentorSearchingResponse> GetMentorBySearchText(string? searchText);
        public List<MentorSearchingResponse> GetAllMentors();
    }
}
