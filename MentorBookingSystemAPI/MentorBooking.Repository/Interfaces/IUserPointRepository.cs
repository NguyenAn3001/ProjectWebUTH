namespace MentorBooking.Repository.Interfaces;

public interface IUserPointRepository
{
    Task<bool> SetUserPoint(Guid userId, int point);
}