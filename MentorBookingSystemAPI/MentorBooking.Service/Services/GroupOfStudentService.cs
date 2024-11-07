using System.Text.RegularExpressions;
using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Service.Services;

public class GroupOfStudentService : IGroupOfStudentService
{
    private readonly IProjectGroupRepository _projectGroupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IStudentGroupRepository _studentGroupRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ApplicationDbContext _dbContext;

    public GroupOfStudentService(IProjectGroupRepository projectGroupRepository, IUserRepository userRepository, IStudentGroupRepository studentGroupRepository, IStudentRepository studentRepository, ApplicationDbContext dbContext)
    {
        _projectGroupRepository = projectGroupRepository;
        _userRepository = userRepository;
        _studentGroupRepository = studentGroupRepository;
        _studentRepository = studentRepository;
        _dbContext = dbContext;
    }

    public async Task<ApiResponse> GetAllGroupsAsync(Guid studentId)
    {
        var groups = await _dbContext.ProjectGroups
            .Where(group => group.StudentGroups.Any(student => student.StudentId == studentId)).Select(gr => new GetManyGroupModelResponse()
            {
                GroupId = gr.GroupId,
                GroupName = gr.GroupName,
                Topic = gr.Topic,
                Creator = gr.StudentGroups.Where(s => s.StudentId == gr.CreateBy)
                    .Select(y => new MemberOfGroupModelResponse()
                    {
                        StudentId = y.StudentId,
                        Name = y.Student.User.FirstName + " " + y.Student.User.LastName,
                        Email = y.Student.User.Email,
                        UserName = y.Student.User.UserName
                    }).SingleOrDefault(),
                Members = gr.StudentGroups
                    .Where(y => y.StudentId != gr.CreateBy)
                    .Select(y => new MemberOfGroupModelResponse()
                    {
                        StudentId = y.StudentId,
                        Name = y.Student.User.FirstName + " " + y.Student.User.LastName,
                        Email = y.Student.User.Email,
                        UserName = y.Student.User.UserName
                    }).ToList()
            }).ToListAsync();
        
        if (groups.Count == 0)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "No groups found"
            };
        // List<GetManyGroupModelResponse> result = new List<GetManyGroupModelResponse>();
        // foreach (var group in groups)
        // {
        //     var members = group.StudentGroups.Select(sg => new MemberOfGroupModelResponse()
        //     {
        //         StudentId = sg.StudentId,
        //         UserName = sg.Student.User.UserName,
        //         Email = sg.Student.User.Email,
        //         Name = sg.Student.User.FirstName + " " + sg.Student.User.LastName
        //     }).ToList();
        //     result.Add(new GetManyGroupModelResponse()
        //     {
        //         GroupId = group.GroupId,
        //         Topic = group.Topic,
        //         GroupName = group.GroupName,
        //         Members = members
        //     });
        // }

        return new ApiResponse()
        {
            Status = "Success",
            Message = "Groups found",
            Data = groups!
        };
    }

    public async Task<ApiResponse> GetYourCreatedGroupAsync(Guid studentId)
    {
        try
        {
            var groups = await _dbContext.ProjectGroups
                .Where(group => group.StudentGroups.Any(student => student.StudentId == studentId && student.StudentId == group.CreateBy)).Select(gr => new GetManyGroupModelResponse()
                {
                    GroupId = gr.GroupId,
                    GroupName = gr.GroupName,
                    Topic = gr.Topic,
                    Creator = gr.StudentGroups.Where(s => s.StudentId == gr.CreateBy)
                        .Select(y => new MemberOfGroupModelResponse()
                        {
                            StudentId = y.StudentId,
                            Name = y.Student.User.FirstName + " " + y.Student.User.LastName,
                            Email = y.Student.User.Email,
                            UserName = y.Student.User.UserName
                        }).SingleOrDefault(),
                    Members = gr.StudentGroups
                        .Where(y => y.StudentId != gr.CreateBy)
                        .Select(y => new MemberOfGroupModelResponse()
                        {
                            StudentId = y.StudentId,
                            Name = y.Student.User.FirstName + " " + y.Student.User.LastName,
                            Email = y.Student.User.Email,
                            UserName = y.Student.User.UserName
                        }).ToList()
                }).ToListAsync();
            if (groups.Count == 0)
                return new ApiResponse()
                {
                    Status = "NotFound",
                    Message = "No groups found"
                };
            return new ApiResponse()
            {
                Status = "Success",
                Message = "Groups found",
                Data = groups!
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Can't get your group, please try again."
            };
        }
        
    }

    public async Task<ApiResponse> DeleteGroupAsync(Guid studentId, Guid groupId)
    {
        var deleteResponse = await _projectGroupRepository.DeleteProjectGroupAsync(studentId, groupId);
        if (!deleteResponse)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Can't delete group, please try again."
            };
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Deleted group successfully."
        };
    }


    public async Task<ApiResponse> UpdateGroupAsync(Guid studentId, Guid groupId, CreateGroupModelRequest updateGroupRequest)
    {
        var groupToUpdate = await _projectGroupRepository.GetProjectGroupById(groupId);
        if (groupToUpdate == null)
            return new ApiResponse()
            {
                Status = "NotFound",
                Message = "Group not found, please try again."
            };
        var updateProjectGroup = new ProjectGroup()
        {
            GroupName = updateGroupRequest.GroupName,
            Topic = updateGroupRequest.Topic
        };
        var updateResponse = await _projectGroupRepository.UpdateProjectGroupAsync(studentId, groupId, updateProjectGroup);
        if (!updateResponse)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Can't update group, please try again."
            };
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Group updated successfully."
        };
    }

    public async Task<ApiResponse> DeleteStudentMemberAsync(Guid userId, Guid groupId, Guid studentIdToRemove)
    {
        var groupExisting = _projectGroupRepository.GetProjectGroupsCreatedByStudentId(userId);
        if (groupExisting.Count == 0 || groupExisting.All(x => x.GroupId != groupId))
            return new ApiResponse()
            {
                Status = "Unauthorized",
                Message = "Unauthorized for delete student member."
            };
        // if (groupExisting.All(x => x.GroupId != groupId))
        //     return new ApiResponse()
        //     {
        //         Status = "NotFound",
        //         Message = "Group not found, please try again."
        //     };
        var deleteResponse = await _projectGroupRepository.DeleteGroupMemberAsync(groupId, studentIdToRemove);
        if (!deleteResponse)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Can't delete student member."
            };
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Member deleted successfully."
        };
    }

    public async Task<ApiResponse> CreateGroupAsync(Guid studentId, CreateGroupModelRequest createGroupRequest)
    {
        ProjectGroup projectGroup = new ProjectGroup()
        {
            GroupId=Guid.NewGuid(),
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