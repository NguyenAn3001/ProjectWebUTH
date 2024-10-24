using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MentorBooking.Service.Services
{
    public class MentorServices : IMentorServices
    {
        private readonly ApplicationDbContext _db;
        public MentorServices(ApplicationDbContext usersDbcontext)
        {
            _db = usersDbcontext;
        }
        private MentorSearchingResponse ConvertMentorToMentorResponse(Mentor mentor)
        {
            MentorSearchingResponse searchMentorRespone = mentor.ToMentorSearchingResponse();
            return searchMentorRespone;
        }
        public List<MentorSearchingResponse> GetAllMentors()
        {
            return _db.Mentors.ToList()
                 .Select(temp => ConvertMentorToMentorResponse(temp)).ToList();
        }

        public List<MentorSearchingResponse> GetMentorBySearchText(string? searchText)
        {
            List<MentorSearchingResponse> allMentors = GetAllMentors();
            List<MentorSearchingResponse> matchingMentors = allMentors;
            if (string.IsNullOrEmpty(searchText)) return matchingMentors;

            matchingMentors = allMentors
                .Where(temp => (
                    !string.IsNullOrEmpty(temp.FirstName) ?
                    temp.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true)|| 
                    (!string.IsNullOrEmpty(temp.LastName) ?
                    temp.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true)).ToList();

            return matchingMentors;
        }
    }
}
