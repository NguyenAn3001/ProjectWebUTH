using MentorBooking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class MentorProfile
    {
        public Guid? MentorId { get; set; }
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public byte? ExperienceYears { get; set; }
        public string? MentorDescription {  get; set; }
        public DateTime CreatAt { get; set; }
        public List<string>? Skills { get; set; }
        public List<string>? comment { get; set; }
        public List<Guid>? StudentId { get; set; }
        public List<string>? StudentName { get; set; }

    }
    public static class MentorProfileExtension
    { 
        public static MentorProfile mentorProfile(this Mentor mentor)
        {
            return new MentorProfile
            {
                MentorId = mentor.MentorId,
                ExperienceYears = mentor.ExperienceYears,
                CreatAt = mentor.CreateAt,
                MentorDescription = mentor.MentorDescription
            };
        }
    }
}
