namespace MentorBooking.Service.DTOs.Response;

public class ProjectProgressModelResponse
{
    public Guid ProgressId { get; set; }
    public Guid? SessionId { get; set; }
    public string? Image { get; set; }
    public string? MentorName { get; set; }
    public string? MentorEmail { get; set; }
    public string? GroupName { get; set; }
    public string? Description { get; set; }
    public DateTime CreatAt { get; set; }
}