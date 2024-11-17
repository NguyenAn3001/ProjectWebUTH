using AutoMapper;
using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
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
            var Query = _db.Users
                .FirstOrDefault(temp => temp.Id == mentor.UserId);
            var mentorSkill=_db.MentorSkills
                .Where(temp=>temp.MentorId== mentor.UserId).ToList();
            MentorSearchingResponse results = new MentorSearchingResponse();
            {
                results.MentorId = mentor.MentorId;
                results.FirstName = Query.FirstName;
                results.LastName = Query.LastName;
                results.Image= Query.Image;
            }
            foreach(var aMentorSkill in mentorSkill)
            {
                var SkillQuery = _db.Skills
                    .FirstOrDefault(temp => temp.SkillId == aMentorSkill.SkillId);
                    
                   
                results.SkillName.Add(SkillQuery.Name);
            }
            return results;
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
                    temp.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) : (!string.IsNullOrEmpty(temp.LastName) ? 
                    (temp.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) : true)
                    )).ToList();
            return matchingMentors;
        }

        public List<MentorSearchingResponse> GetSortMentor(List<MentorSearchingResponse> allMentors,string? sortBy)
        {
            allMentors = GetAllMentors();
            List<MentorSearchingResponse> sortMentor = new List<MentorSearchingResponse>();
            switch(sortBy)
            {
                case SkillOptions.php:
                    foreach(var aMentor in allMentors)
                    {
                        foreach(var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill,sortBy,true)==0) sortMentor.Add(aMentor);
                        }    
                    }
                    break;
                case SkillOptions.python:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                case SkillOptions.js:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                case SkillOptions.java:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                case SkillOptions.fullStack:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                case SkillOptions.frontend:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                case SkillOptions.cShape:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                case SkillOptions.c:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                case SkillOptions.cPP:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                case SkillOptions.backend:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                case SkillOptions.frontEndLanguage:
                    foreach (var aMentor in allMentors)
                    {
                        foreach (var skill in aMentor.SkillName)
                        {
                            if (string.Compare(skill, sortBy, true) == 0) sortMentor.Add(aMentor);
                        }
                    }
                    break;
                default: sortMentor = allMentors; break;
            }    
            return sortMentor;
        }
    }
}
