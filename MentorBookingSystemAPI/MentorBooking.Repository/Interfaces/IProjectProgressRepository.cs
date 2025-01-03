﻿using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IProjectProgressRepository
{
    Task<bool> CreateProjectProgressAsync(ProjectProgress projectProgress);
    Task<bool> UpdateProjectProgressAsync(ProjectProgress projectProgress);
    Task<bool> DeleteProjectProgressAsync(ProjectProgress projectProgress);
    Task<ProjectProgress?> GetProjectProgressAsync(Guid progressId);
    Task<List<ProjectProgress>?> GetAllProjectProgressAsync(Guid sessionId);
    Task<string> GetMentorIdProjectProgressesAsync(Guid sessionId);
    Task<ProjectGroup> GetGroupAsync(Guid progressId);
    Task<int> GetMaxIndexBySessionIdAsync(Guid sessionId);
    Task UpdateIndexAfterDeleteAsync(Guid sessionId);
}