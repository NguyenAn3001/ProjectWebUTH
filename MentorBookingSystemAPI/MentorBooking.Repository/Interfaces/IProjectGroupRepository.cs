using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IProjectGroupRepository
{
    Task<bool> CreateNewProjectGroupAsync(ProjectGroup projectGroup);
    List<ProjectGroup> GetProjectGroupsCreatedByStudentId(Guid studentId);
}