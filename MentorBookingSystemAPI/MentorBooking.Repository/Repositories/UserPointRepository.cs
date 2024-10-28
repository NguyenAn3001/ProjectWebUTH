using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;

namespace MentorBooking.Repository.Repositories;

public class UserPointRepository : IUserPointRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserPointRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> SetUserPoint(Guid userId, int point)
    {
        try
        {
            UserPoint initPoint = new UserPoint()
            {
                UserId = userId,
                PointsBalance = point
            };
            if (_dbContext.UserPoints.FirstOrDefault(x => x.UserId == userId) == null)
            {
                await _dbContext.UserPoints.AddAsync(initPoint);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}