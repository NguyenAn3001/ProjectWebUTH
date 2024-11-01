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
}