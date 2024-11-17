using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Services
{
    public class StudentProfilesServices : IStudentProfilesServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentRepository _studentRepository;
        public StudentProfilesServices(IUserRepository userRepository, IStudentRepository studentRepository)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
        }
        public async Task<ApiResponse?> StudentProfiles(Guid StudentId)
        {
            var userProfiles = await _userRepository.FindByIdAsync(StudentId.ToString());
            var studentProfiles =await _studentRepository.GetStudentByIdAsync(StudentId);
            if(userProfiles == null && studentProfiles==null) 
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Your StudentId is not found"
                };
            }
            if(studentProfiles==null)
            {
                return new ApiResponse()
                {
                    Status = "Success",
                    Message = "Found",
                    Data = new StudentProfilesResponse()
                    {
                        StudentId = StudentId,
                        Name = null,
                        Phone=null,
                        Email=null,
                        CreatedAt=null
                    }
                };
            }    
            return new ApiResponse()
            {
                Status = "Success",
                Message = "Found",
                Data = new StudentProfilesResponse()
                {
                    StudentId = StudentId,
                    Name = userProfiles.FirstName + " " + userProfiles.LastName,
                    Phone = userProfiles.PhoneNumber,
                    Email = userProfiles.Email,
                    CreatedAt = studentProfiles.CreateAt,
                }
            };
        }
    }
}
