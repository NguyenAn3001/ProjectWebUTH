using System.Text.RegularExpressions;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.Interfaces;

public interface IGroupOfStudentService
{
    Task<ApiResponse> CreateGroupAsync(Guid studentId, CreateGroupModelRequest createGroupRequest);
    Task<ApiResponse> AddStudentToGroupAsync(Guid studentId, Guid groupId, List<StudentToAddGroupModelRequest> students);
    // Task<Group> UpdateGroupAsync(Group group);
    // Task DeleteGroupAsync(Group group);
    Task<ApiResponse> GetAllGroupsAsync(Guid studentId);
    Task<ApiResponse> GetYourCreatedGroupAsync(Guid studentId);
    // Task<Group> GetGroupByIdAsync(Group group);
    // Task<List<StudentGroupResponse>> GetStudentGroupResponses(Group group);
    Task<ApiResponse> DeleteGroupAsync(Guid studentId, Guid groupId);
    Task<ApiResponse> UpdateGroupAsync(Guid studentId, Guid groupId, CreateGroupModelRequest updateGroupRequest);
    Task<ApiResponse> DeleteStudentMemberAsync(Guid userId, Guid groupId, Guid studentIdToRemove);
}