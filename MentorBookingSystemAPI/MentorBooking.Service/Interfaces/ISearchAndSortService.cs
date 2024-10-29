using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Interfaces
{
    public interface ISearchAndSortService
    {
        public List<MentorSearchingResponse> GetMentorBySearchText(string? searchText);
        public List<MentorSearchingResponse> GetAllMentors();
        public List<MentorSearchingResponse> GetSortMentor(List<MentorSearchingResponse> allMentors,string? sortBy);
    }
}
