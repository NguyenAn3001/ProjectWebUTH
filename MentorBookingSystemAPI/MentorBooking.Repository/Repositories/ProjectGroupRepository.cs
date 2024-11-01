using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;

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
}