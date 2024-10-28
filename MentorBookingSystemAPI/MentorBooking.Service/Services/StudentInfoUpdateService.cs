using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;

namespace MentorBooking.Service.Services;

public class StudentInfoUpdateService : IUserInformationUpdate
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUserPointRepository _userPointRepository;
    private const int PointDefaultForStudent = 100;

    public StudentInfoUpdateService(IStudentRepository studentRepository,
        IUserPointRepository userPointRepository)
    {
        _studentRepository = studentRepository;
        _userPointRepository = userPointRepository;
    }

    public async Task<bool> UpdateInformationUser(Users user, object request)
    {
        StudentInformationModelRequest? studentRequest = request as StudentInformationModelRequest;
        user.FirstName = studentRequest?.FirstName;
        user.LastName = studentRequest?.LastName;
        user.PhoneNumber = studentRequest?.Phone;
        Student student = new Student()
        {
            StudentId = user.Id,
            UserId = user.Id,
            CreateAt = DateTime.Now
        };
        // if (_dbContext.Students.FirstOrDefault(x => x.UserId == user.Id) == null)
        // {
        //     await _dbContext.Students.AddAsync(student);         
        //     await _dbContext.SaveChangesAsync();
        // }
        if (!await _studentRepository.AddInformationStudentAsync(student))
        {
            return false;
        }
        // UserPoint initPoint = new UserPoint()
        // {
        //     UserId = user.Id,
        //     PointsBalance = 100
        // };
        // if (_dbContext.UserPoints.FirstOrDefault(x => x.UserId == user.Id) == null)
        // {
        //     await _dbContext.UserPoints.AddAsync(initPoint);
        //     await _dbContext.SaveChangesAsync();
        // }

        if (!await _userPointRepository.SetUserPoint(student.UserId, PointDefaultForStudent))
            return false;
        return true;
    }
}