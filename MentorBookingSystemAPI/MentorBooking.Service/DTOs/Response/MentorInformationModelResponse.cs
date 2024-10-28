namespace MentorBooking.Service.DTOs.Request;

public class MentorInformationModelResponse
{
    public Guid MentorId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public string? Phone { get; set; }
    public byte? ExperienceYears { get; set; }
    public string? MentorDescription { get; set; }
    public List<string>? Skills { get; set; }
    public DateTime CreatedAt { get; set; }
}