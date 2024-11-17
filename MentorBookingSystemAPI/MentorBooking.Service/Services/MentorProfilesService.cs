using MentorBooking.Repository.Interfaces;
using MentorBooking.Repository.Repositories;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Services
{
    public class MentorProfilesService : IMentorProfilesService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMentorRepository _mentorRepository;
        private readonly IMentorSkillRepository _mentorskillRepository;
        private readonly ISkillRepository _skilllRepository;
        public MentorProfilesService(IUserRepository userRepository, IMentorRepository mentorRepository,IMentorSkillRepository mentorSkillRepository,ISkillRepository skillRepository)
        {
            _userRepository = userRepository;
            _mentorRepository = mentorRepository;
            _skilllRepository = skillRepository;
            _mentorskillRepository=mentorSkillRepository;
        }
        public async Task<ApiResponse?> MentorProfiles(Guid MentorId)
        {
            var userProfiles =await _userRepository.FindByIdAsync(MentorId.ToString());
            var mentorProfiles = await _mentorRepository.GetMentorByIdAsync(MentorId);
            if(userProfiles == null && mentorProfiles == null) 
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Not Found"
                };
            }
            var mentorSkills = _mentorskillRepository.GetMentorSkillByIdAsync(MentorId);
            List<string> skillName = new List<string>();
            foreach (var item in mentorSkills)
            {
                var SkillName =await _skilllRepository.GetSkillByIdAsync(item.SkillId);
                if(SkillName != null)
                {
                    skillName.Add(SkillName.Name);
                }    
            }
            if (mentorProfiles == null)
            {
                return new ApiResponse()
                {
                    Status = "Success",
                    Message = "Found",
                    Data = new MentorProfilesResponse()
                    {
                        MentorId = MentorId,
                        Name = null,
                        Phone = null,
                        Email = null,
                        ExperienceYears = null,
                        MentorDescription = null,
                        CreatedAt = null,
                        Skills = skillName
                    }
                };
            }
                return new ApiResponse()
            {
                Status = "Success",
                Message = "Found",
                Data = new MentorProfilesResponse()
                {
                    MentorId = MentorId,
                    Name = userProfiles.FirstName + " " + userProfiles.LastName,
                    Phone = userProfiles.PhoneNumber,
                    Email = userProfiles.Email,
                    ExperienceYears = mentorProfiles.ExperienceYears,
                    MentorDescription = mentorProfiles.MentorDescription,
                    CreatedAt = mentorProfiles.CreateAt,
                    Skills=skillName
                }
            };
        }
    }
}
