using System.Text.RegularExpressions;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.Interfaces;

public interface IGroupOfStudentService
{
    Task<ApiResponse> CreateGroupAsync(Guid studentId, CreateGroupModelRequest createGroupRequest);
    Task<ApiResponse> AddStudentToGroupAsync(Guid studentId, int groupId, List<StudentToAddGroupModelRequest> students);
    // Task<Group> UpdateGroupAsync(Group group);
    // Task DeleteGroupAsync(Group group);
    Task<List<GetManyGroupModelResponse>> GetAllGroupsAsync(Guid studentId);
    Task<List<GetManyGroupModelResponse>> GetYourCreatedGroupAsync(Guid studentId);
    // Task<Group> GetGroupByIdAsync(Group group);
    // Task<List<StudentGroupResponse>> GetStudentGroupResponses(Group group);
}