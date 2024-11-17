using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IProjectGroupRepository
{
    Task<bool> CreateNewProjectGroupAsync(ProjectGroup projectGroup);
    List<ProjectGroup> GetProjectGroupsCreatedByStudentId(Guid studentId);
    Task<bool> DeleteProjectGroupAsync(Guid studentId, Guid projectGroupId);
    Task<bool> UpdateProjectGroupAsync(Guid studentId, Guid groupId, ProjectGroup projectGroup);
    Task<ProjectGroup?> GetProjectGroupById(Guid projectGroupId);
    Task<bool> DeleteGroupMemberAsync(Guid groupId, Guid userId);
}