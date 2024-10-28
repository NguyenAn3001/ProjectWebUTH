using AutoMapper;
using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.AutoMapper;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Enum;
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
    public class SearchAndSortService : ISearchAndSortService
    {
        private readonly ApplicationDbContext _db;
        public SearchAndSortService(ApplicationDbContext mentorDbcontext)
        {
            _db = mentorDbcontext;
        }
        private MentorSearchingResponse ConvertMentorToMentorSearchingResponse(Mentor mentor)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            var mapper = config.CreateMapper();
            return mapper.Map<MentorSearchingResponse>(mentor);
        }
        public List<MentorSearchingResponse> GetAllMentors()
        {
            return _db.Mentors.ToList()
                 .Select(temp => ConvertMentorToMentorSearchingResponse(temp)).ToList();
        }

        public List<MentorSearchingResponse> GetMentorBySearchText(string? searchText)
        {
            List<MentorSearchingResponse> allMentors = GetAllMentors();
            List<MentorSearchingResponse> matchingMentors = allMentors;
            if (string.IsNullOrEmpty(searchText)) return matchingMentors;

            matchingMentors = allMentors
                .Where(temp => (
                    !string.IsNullOrEmpty(temp.FirstName) ?
                    temp.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) : 
                    (!string.IsNullOrEmpty(temp.LastName) ?
                    temp.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true))
                    ).ToList();

            return matchingMentors;
        }

        public List<MentorSearchingResponse> GetSortMentor(List<MentorSearchingResponse> allMentors, SortOptions sortType)
        {
            allMentors = GetAllMentors();
            List<MentorSearchingResponse> sortMentor = (sortType) switch
            {
                (SortOptions.ASC) => allMentors.OrderBy(temp => temp.FirstName, StringComparer.OrdinalIgnoreCase).ToList(),
                (SortOptions.DESC) => allMentors.OrderByDescending(temp => temp.FirstName, StringComparer.OrdinalIgnoreCase).ToList(),
                _ => allMentors
            };
            return sortMentor;
        }
    }
}
