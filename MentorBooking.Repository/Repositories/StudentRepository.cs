using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Repository.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StudentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> AddInformationStudentAsync(Student student)
    {
        try
        {
            var studentResponse = _dbContext.Students.Where(x => x.StudentId == student.StudentId).ToList();
            if (studentResponse.Count > 0)
            {
                _dbContext.RemoveRange(studentResponse);
                await _dbContext.Students.AddAsync(student);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<Student?> GetStudentByIdAsync(Guid studentId)
    {
        return await _dbContext.Students.SingleOrDefaultAsync(s => s.StudentId == studentId);
    }
}