using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.Interfaces;

public interface IUpdateInformationService
{
    Task<ApiResponse> UpdateMentorInformationAsync(Guid mentorId, MentorInformationModelRequest request);
    Task<ApiResponse> UpdateStudentInformationAsync(Guid studentId, StudentInformationModelRequest request);
}