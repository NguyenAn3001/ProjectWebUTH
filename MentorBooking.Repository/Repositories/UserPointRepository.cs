using System.Data.SqlTypes;
using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Repository.Repositories;

public class UserPointRepository : IUserPointRepository
{
    private readonly ApplicationDbContext _dbContext;
    private const bool Increase = true;
    private const bool Decrease = false;
    public UserPointRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> SetUserPoint(Guid userId, int point, string descriptionTransaction = "Initial point.")
    {
        try
        {
            UserPoint initPoint = new UserPoint()
            {
                UserId = userId,
                PointsBalance = point
            };
            int pointChanged;
            PointTransaction pointTransaction;
            if ((_dbContext.UserPoints.FirstOrDefault(x => x.UserId == userId)) == null)
            {
                pointChanged = point;
                await _dbContext.UserPoints.AddAsync(initPoint);
                await _dbContext.SaveChangesAsync();
                pointTransaction = new PointTransaction()
                {
                    UserId = userId,
                    CreateAt = DateTime.Now,
                    PointsChanged = pointChanged,
                    TransactionId = Guid.NewGuid(),
                    TransactionType = Increase,
                    Description = descriptionTransaction
                };
                await _dbContext.PointTransactions.AddAsync(pointTransaction);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            var pointExists = await GetUserPoint(userId);
            pointChanged = point - pointExists.PointsBalance!.Value;
            pointExists.PointsBalance = point;
            await _dbContext.SaveChangesAsync();
            pointTransaction = new PointTransaction()
            {
                UserId = userId,
                CreateAt = DateTime.Now,
                PointsChanged = Math.Abs(pointChanged),
                TransactionId = Guid.NewGuid(),
                TransactionType = pointChanged >= 0 ? Increase : Decrease,
                Description = descriptionTransaction
            };
            await _dbContext.PointTransactions.AddAsync(pointTransaction);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<UserPoint> GetUserPoint(Guid userId)
    {
        try
        {
            var userPoint = await _dbContext.UserPoints.FirstOrDefaultAsync(x => x.UserId == userId);
            return userPoint!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null!;
        }
    }

    public List<PointTransaction> GetAllPointTransaction()
    {
        var listPointTransaction = _dbContext.PointTransactions.ToList();
        return listPointTransaction;
    }

    public async Task<bool> DeletePointTransaction(Guid PointTransactionId)
    {
        try
        {
            var pointTrans = await _dbContext.PointTransactions.SingleOrDefaultAsync(temp => temp.TransactionId == PointTransactionId);
            if (pointTrans == null)
            {
                return false;
            }
            _dbContext.PointTransactions.Remove(pointTrans);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        
    }
}