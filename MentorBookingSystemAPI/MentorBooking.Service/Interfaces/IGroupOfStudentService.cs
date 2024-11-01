using System.Text.RegularExpressions;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.Interfaces;

public interface IGroupOfStudentService
{
    Task<ApiResponse> CreateGroupAsync(Guid studentId, CreateGroupModelRequest createGroupRequest);
    // Task<Group> UpdateGroupAsync(Group group);
    // Task DeleteGroupAsync(Group group);
    // Task<Group> GetGroupAsync(Group group);
    // Task<List<Group>> GetGroupsAsync(List<Group> groups);
    // Task<Group> GetGroupByIdAsync(Group group);
    // Task<List<StudentGroupResponse>> GetStudentGroupResponses(Group group);
    Task<ApiResponse> AddStudentToGroupAsync(Guid studentId, Guid groupId, List<StudentToAddGroupModelRequest> students);
}