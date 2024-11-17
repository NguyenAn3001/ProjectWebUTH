namespace MentorBooking.Service.DTOs.Response;

public class MemberOfGroupModelResponse
{
    public Guid StudentId { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}