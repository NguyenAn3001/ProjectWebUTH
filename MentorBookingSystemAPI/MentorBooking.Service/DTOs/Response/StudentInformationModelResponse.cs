namespace MentorBooking.Service.DTOs.Response;

public class StudentInformationModelResponse
{
    public Guid StudentId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
}