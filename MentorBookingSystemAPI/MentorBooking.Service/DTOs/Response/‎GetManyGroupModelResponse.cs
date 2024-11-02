using System.Reflection;

namespace MentorBooking.Service.DTOs.Response;

public class GetManyGroupModelResponse
{
    public Guid GroupId { get; set; }
    public string? GroupName { get; set; }
    public string? Topic { get; set; }
    public MemberOfGroupModelResponse? Creator { get; set; }
    public List<MemberOfGroupModelResponse>? Members { get; set; }
}