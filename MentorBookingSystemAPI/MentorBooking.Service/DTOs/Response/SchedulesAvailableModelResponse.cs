namespace MentorBooking.Service.DTOs.Response;

public class SchedulesAvailableModelResponse
{
    public Guid? ScheduleAvailableId { get; set; }
    public Guid? MentorId { get; set; }
    public DateOnly FreeDay { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}