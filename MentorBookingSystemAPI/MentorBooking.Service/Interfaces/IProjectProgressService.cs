using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.Interfaces;

public interface IProjectProgressService
{
    Task<ApiResponse> CreateProjectProgressAsync(CreateProjectProgressModelRequest createProjectProgressRequest);
    Task<ApiResponse> UpdateProjectProgressAsync(UpdateProjectProgressModelRequest updateProjectProgressRequest);
    Task<ApiResponse> DeleteProjectProgressAsync(Guid projectProgressId);
    Task<ApiResponse> GetProjectProgressAsync(Guid projectProgressId);
    Task<ApiResponse> GetAllProjectProgressAsync(Guid sessionId);
}