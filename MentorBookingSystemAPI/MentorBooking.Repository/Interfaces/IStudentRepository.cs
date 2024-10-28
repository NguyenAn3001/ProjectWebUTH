using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IStudentRepository
{
    Task<bool> AddInformationStudentAsync(Student student);
}