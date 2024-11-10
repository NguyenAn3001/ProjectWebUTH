using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;

namespace MentorBooking.Repository.Repositories;

public class StudentGroupRepository : IStudentGroupRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StudentGroupRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> AddStudentGroupAsync(StudentGroup studentGroup)
    {
        try
        {
           _dbContext.StudentGroups.Add(studentGroup);
           await _dbContext.SaveChangesAsync();
           return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> AddListStudentToGroupAsync(List<StudentGroup> studentGroups)
    {
        try
        {
            await _dbContext.StudentGroups.AddRangeAsync(studentGroups);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    public List<StudentGroup>? GetAllStudentInGroup(Guid GroupId)
    {
        var listStudent = _dbContext.StudentGroups.Where(temp=>temp.GroupId==GroupId).ToList();
        if(listStudent.Count()==0)
        {
            return null;
        }
        return listStudent;
    }

    public List<StudentGroup>? GetListStudentInGroup(Guid StudentId)
    {
        var listGroup=_dbContext.StudentGroups.Where(temp=>temp.StudentId==StudentId).ToList();
        if (listGroup.Count()==0)
        {
            return null;
        }
        return listGroup;
    }
}