using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;

namespace MentorBooking.Service.Services;

public class UpdateInformationHandler : IUpdateInformationService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserInformationFactory _userInformationFactory;

    public UpdateInformationHandler(IUserRepository userRepository, IUserInformationFactory userInformationFactory)
    {
        _userRepository = userRepository;
        _userInformationFactory = userInformationFactory;
    }
    public async Task<ApiResponse> UpdateMentorInformationAsync(Guid userId, MentorInformationModelRequest request)
    {
        try
        {
            var user = await _userRepository.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Mentor does not exist."
                };
            }
            var updateInformationUser = await _userInformationFactory.CreateUserInformation("Mentor").UpdateInformationUser(user!, request);
            if (!updateInformationUser)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Mentor information update failed."
                };
            }
            return new ApiResponse()
            {
                Status = "Success",
                Message = "Mentor Information Created Successfully.",
                Data = new MentorInformationModelResponse()
                {
                    MentorId = user!.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    MentorDescription = request.MentorDescription,
                    ExperienceYears = request.ExperienceYears,
                    ImageUrl = request.ImageUrl,
                    CreatedAt = request.CreatedAt,
                    Skills = request.Skills,
                    Phone = request.Phone
                }
            };            
        }
        catch (Exception e)
        {
            return new ApiResponse()
            {
                Status = "ServerError",
                Message = e.Message
            };
        }
    }

    public async Task<ApiResponse> UpdateStudentInformationAsync(Guid studentId, StudentInformationModelRequest request)
    {
        try
        {
            var user = await _userRepository.FindByIdAsync(studentId.ToString());
            if (user == null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Student does not exist."
                };
            }
            var updateInformationUser = await _userInformationFactory.CreateUserInformation("Student").UpdateInformationUser(user!, request);
            if (!updateInformationUser)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Student information update failed."
                };
            }
            return new ApiResponse()
            {
                Status = "Success",
                Message = "Student Information Created Successfully.",
                Data = new StudentInformationModelResponse()
                {
                    StudentId = user!.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ImageUrl = request.ImageUrl,
                    CreatedAt = request.CreatedAt,
                    Phone = request.Phone
                }
            };
        }
        catch (Exception e)
        {
            return new ApiResponse()
            {
                Status = "ServerError",
                Message = e.Message
            };
        }
    }
}