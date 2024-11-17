using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Repository.Repositories;

public class ProjectGroupRepository : IProjectGroupRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStudentGroupRepository _studentGroupRepository;

    public ProjectGroupRepository(ApplicationDbContext dbContext, IStudentGroupRepository studentGroupRepository)
    {
        _dbContext = dbContext;
        _studentGroupRepository = studentGroupRepository;
    }
    public async Task<bool> CreateNewProjectGroupAsync(ProjectGroup projectGroup)
    {
        try
        {
            _dbContext.ProjectGroups.Add(projectGroup);
            await _dbContext.SaveChangesAsync();
            StudentGroup addNewStudentGroup = new StudentGroup()
            {
                GroupId = projectGroup.GroupId,
                StudentId = projectGroup.CreateBy,
                JoinAt = DateTime.Now,
            };
            if (!await _studentGroupRepository.AddStudentGroupAsync(addNewStudentGroup))
            {
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public List<ProjectGroup> GetProjectGroupsCreatedByStudentId(Guid studentId)
    {
        var projectGroups = _dbContext.ProjectGroups.Where(x => x.CreateBy == studentId).ToList();
        return projectGroups;
    }

    public async Task<bool> DeleteProjectGroupAsync(Guid studentId, Guid projectGroupId)
    {
        try
        {
            var group = await _dbContext.ProjectGroups.FirstOrDefaultAsync(x => x.GroupId == projectGroupId && x.CreateBy == studentId);
            if (group == null)
                return false;
            _dbContext.ProjectGroups.Remove(group);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> UpdateProjectGroupAsync(Guid studentId, Guid groupId, ProjectGroup projectGroup)
    {
        try
        {
            var group = await _dbContext.ProjectGroups.FirstOrDefaultAsync(x => x.GroupId == groupId && studentId == x.CreateBy);
            if (group == null)
                return false;
            
            group.GroupName = projectGroup.GroupName;
            group.Topic = projectGroup.Topic;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ProjectGroup?> GetProjectGroupById(Guid projectGroupId)
    {
        var projectGroup = await _dbContext.ProjectGroups.FirstOrDefaultAsync(x => x.GroupId == projectGroupId);
        return projectGroup;
    }

    public async Task<bool> DeleteGroupMemberAsync(Guid groupId, Guid studentId)
    {
        try
        {
// var existingGroup = await _dbContext.ProjectGroups.FirstOrDefaultAsync(x => x.GroupId == groupId);
            // if (existingGroup == null)
            //     return false;
            // var isStudentInGroup = existingGroup.StudentGroups.Any(x => x.StudentId == userId);
            // if (!isStudentInGroup)
            //     return false;
            // _dbContext.StudentGroups.Remove(existingGroup.StudentGroups.First(x => x.StudentId == userId));
            var existingStudentGroup = await _dbContext.StudentGroups.FirstOrDefaultAsync(x => x.GroupId == groupId && x.StudentId == studentId);
            if (existingStudentGroup == null)
                return false;
            _dbContext.StudentGroups.Remove(existingStudentGroup);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
    }
}