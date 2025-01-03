﻿using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;

namespace MentorBooking.Service.Services;

public class ProjectProgressService : IProjectProgressService
{
    private readonly IProjectProgressRepository _projectProgressRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMentorSupportSessionRepository _mentorSupportSessionRepository;

    public ProjectProgressService(IProjectProgressRepository projectProgressRepository, IUserRepository userRepository, IMentorSupportSessionRepository mentorSupportSessionRepository)
    {
        _projectProgressRepository = projectProgressRepository;
        _userRepository = userRepository;
        _mentorSupportSessionRepository = mentorSupportSessionRepository;
    }

    public async Task<ApiResponse> CreateProjectProgressAsync(CreateProjectProgressModelRequest createProjectProgressRequest)
    {
        //if session id not found => fail to create (note modify after)
        if (await _mentorSupportSessionRepository.GetMentorSupportSessionAsync(createProjectProgressRequest.SessionId!
                .Value) is null)
        {
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Session doesn't exist."
            };
        }
        var maxIndex = await _projectProgressRepository.GetMaxIndexBySessionIdAsync(createProjectProgressRequest.SessionId!.Value);
        var projectProgress = new ProjectProgress
        {
            ProgressId = Guid.NewGuid(),
            SessionId = createProjectProgressRequest.SessionId!.Value,
            Description = createProjectProgressRequest.Description,
            ProgressIndex = maxIndex + 1,
            UpdateAt = DateTime.Now
        };
        var session = await _mentorSupportSessionRepository.GetMentorSupportSessionAsync(projectProgress.SessionId);
        if (projectProgress.ProgressIndex > session?.SessionCount)
        {
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Session is already finished."
            };
        }
        var createResponse = await _projectProgressRepository.CreateProjectProgressAsync(projectProgress);
        if (!createResponse)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Error creating project progress, please try again later."
            };
        
        var mentorExist = await _projectProgressRepository.GetMentorIdProjectProgressesAsync(projectProgress.SessionId);
        var projectProgressResponse = await GetProjectProgressResponse(mentorExist, projectProgress);
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Project progress created successfully.",
            Data = projectProgressResponse
        };
    }

    public async Task<ApiResponse> UpdateProjectProgressAsync(UpdateProjectProgressModelRequest updateProjectProgressRequest)
    {
        var projectProgress = await _projectProgressRepository.GetProjectProgressAsync(updateProjectProgressRequest.ProgressId!.Value);
        if (projectProgress is null)
        {
            return new ApiResponse()
            {
                Status = "NotFound",
                Message = "Project progress not found."
            };
        }
        var updateResponse = await _projectProgressRepository.UpdateProjectProgressAsync(new ProjectProgress()
        {
            ProgressId = projectProgress.ProgressId,
            Description = updateProjectProgressRequest.Description,
            UpdateAt = DateTime.Now
        });
        if (!updateResponse)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Error updating project progress, please try again later."
            };
        var mentorExist = await _projectProgressRepository.GetMentorIdProjectProgressesAsync(projectProgress.SessionId);
        var projectProgressResponse = await GetProjectProgressResponse(mentorExist, projectProgress);
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Project progress updated successfully.",
            Data = projectProgressResponse
        };
    }

    public async Task<ApiResponse> DeleteProjectProgressAsync(Guid projectProgressId)
    {
        var projectProgress = await _projectProgressRepository.GetProjectProgressAsync(projectProgressId);
        if (projectProgress is null)
            return new ApiResponse()
            {
                Status = "NotFound",
                Message = "Project progress not found."
            };
        var deleteResponse = await _projectProgressRepository.DeleteProjectProgressAsync(projectProgress);
        if (!deleteResponse)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Error deleting project progress, please try again later."
            };
        await _projectProgressRepository.UpdateIndexAfterDeleteAsync(projectProgress.SessionId);
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Project progress deleted successfully."
        };
    }

    public async Task<ApiResponse> GetProjectProgressAsync(Guid projectProgressId)
    {
        var projectProgress = await _projectProgressRepository.GetProjectProgressAsync(projectProgressId);
        if (projectProgress is null)
            return new ApiResponse()
            {
                Status = "NotFound",
                Message = "Project progress not found."
            };
        var mentorExist = await _projectProgressRepository.GetMentorIdProjectProgressesAsync(projectProgress.SessionId);
        var projectProgressResponse = await GetProjectProgressResponse(mentorExist, projectProgress);
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Project progress retrieved successfully.",
            Data = projectProgressResponse
        };
    }

    public async Task<ApiResponse> GetAllProjectProgressAsync(Guid sessionId)
    {
        var projectProgressList = await _projectProgressRepository.GetAllProjectProgressAsync(sessionId);
        if (projectProgressList is null)
            return new ApiResponse()
            {
                Status = "NotFound",
                Message = "Project progress not found."
            };
        var projectProgressListResponse = new List<ProjectProgressModelResponse>();
        foreach (var projectProgress in projectProgressList)
        {
            var mentorExist = await _projectProgressRepository.GetMentorIdProjectProgressesAsync(projectProgress.SessionId);
            var projectProgressResponse = await GetProjectProgressResponse(mentorExist, projectProgress);
            projectProgressListResponse.Add(projectProgressResponse);
        }

        return new ApiResponse()
        {
            Status = "Success",
            Message = "All project progress retrieved successfully.",
            Data = projectProgressListResponse
        };
    }
    private async Task<ProjectProgressModelResponse> GetProjectProgressResponse(string mentorId, ProjectProgress projectProgress)
    {
         
        var user = await _userRepository.FindByIdAsync(mentorId);
        var group = await _projectProgressRepository.GetGroupAsync(projectProgress.ProgressId);
        var session = await _mentorSupportSessionRepository.GetMentorSupportSessionAsync(projectProgress.SessionId);
        var progressCount = session?.SessionCount ?? 1;
        var projectProgressResponse = new ProjectProgressModelResponse()
        {
            ProgressId = projectProgress.ProgressId,
            SessionId = projectProgress.SessionId,
            MentorName = $"{user?.FirstName} {user?.LastName}",
            MentorEmail = user?.Email,
            GroupName = group?.GroupName,
            Image = user?.Image,
            Description = projectProgress.Description,
            ProgressIndex = projectProgress.ProgressIndex,
            ProgressCount = progressCount,
            CreatAt = projectProgress.UpdateAt
        };
        return projectProgressResponse;
    }
}