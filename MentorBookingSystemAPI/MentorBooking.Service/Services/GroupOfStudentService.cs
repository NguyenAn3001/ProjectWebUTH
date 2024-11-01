using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;

namespace MentorBooking.Service.Services;

public class GroupOfStudentService : IGroupOfStudentService
{
    private readonly IProjectGroupRepository _projectGroupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IStudentGroupRepository _studentGroupRepository;
    private readonly IStudentRepository _studentRepository;

    public GroupOfStudentService(IProjectGroupRepository projectGroupRepository, IUserRepository userRepository, IStudentGroupRepository studentGroupRepository, IStudentRepository studentRepository)
    {
        _projectGroupRepository = projectGroupRepository;
        _userRepository = userRepository;
        _studentGroupRepository = studentGroupRepository;
        _studentRepository = studentRepository;
    }
    
    public async Task<ApiResponse> CreateGroupAsync(Guid studentId, CreateGroupModelRequest createGroupRequest)
    {
        ProjectGroup projectGroup = new ProjectGroup()
        {
            GroupName = createGroupRequest.GroupName,
            Topic = createGroupRequest.Topic,
            CreateBy = studentId,
            CreateAt = DateTime.Now
        };
        if (!await _projectGroupRepository.CreateNewProjectGroupAsync(projectGroup))
        {
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Creating a new project group failed. Please try again later."
            };
        }

        return new ApiResponse()
        {
            Status = "Success",
            Message = "Group created successfully."
        };
    }

    public async Task<ApiResponse> AddStudentToGroupAsync(Guid studentId, Guid groupId, List<StudentToAddGroupModelRequest> students)
    {
        var groupsCreated = _projectGroupRepository.GetProjectGroupsCreatedByStudentId(studentId);
        if (groupsCreated.Count == 0)
        {
            return new ApiResponse()
            {
                Status = "Error",
                Message = "There are no groups created yet, please create a new project group then try again."
            };
        }

        var groupToAdd = groupsCreated.FirstOrDefault(x => x.GroupId == groupId);
        if (groupToAdd == null)
            return new ApiResponse()
            {
                Status = "Unauthorized",
                Message = "Unauthorized add student to group."
            };
        List<StudentGroup> studentGroups = new List<StudentGroup>();
        foreach (var student in students)
        {
            var studentExist = await _studentRepository.GetStudentByIdAsync(student.StudentId);
            if (studentExist == null)
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = $"Unable to find student with id: {student.StudentId}. Please try again."
                };
            }
                
            studentGroups.Add(new StudentGroup()
            {
                GroupId = groupToAdd.GroupId,
                StudentId = student.StudentId,
                JoinAt = DateTime.Now
            });
        }

        if (!await _studentGroupRepository.AddListStudentToGroupAsync(studentGroups))
        {
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Failed to add students to group."
            };
        }
        List<StudentGroupModelResponse> studentsResponse = new List<StudentGroupModelResponse>();
        foreach (var student in studentGroups)
        {
            var user = await _userRepository.FindByIdAsync(student.StudentId.ToString());
            studentsResponse.Add(new StudentGroupModelResponse()
            {
                GroupId = groupId,
                StudentName = user?.LastName + " " + user?.FirstName,
                JoinAt = student.JoinAt
            });
        }

        return new ApiResponse()
        {
            Status = "Success",
            Message = "Added student to group successfully.",
            Data = studentsResponse
        };
    }
}