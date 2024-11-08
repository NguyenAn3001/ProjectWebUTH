using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IStudentGroupRepository
{
    Task<bool> AddStudentGroupAsync(StudentGroup studentGroup);
    Task<bool> AddListStudentToGroupAsync(List<StudentGroup> studentGroups);
    // Task<bool> UpdateStudentGroupAsync(StudentGroup studentGroup);
    List<StudentGroup>? GetAllStudentInGroup(Guid GroupId);
    List<StudentGroup>? GetListStudentInGroup(Guid StudentId);
}