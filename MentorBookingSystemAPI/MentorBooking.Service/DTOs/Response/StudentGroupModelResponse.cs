using MentorBooking.Repository.Entities;

namespace MentorBooking.Service.DTOs.Response;

public class StudentGroupModelResponse
{
    public int GroupId { get; set; }
    public string? StudentName { get; set; }
    public DateTime JoinAt { get; set; }
}