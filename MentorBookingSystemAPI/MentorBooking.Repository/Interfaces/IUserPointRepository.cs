﻿using MentorBooking.Repository.Entities;

namespace MentorBooking.Repository.Interfaces;

public interface IUserPointRepository
{
    Task<bool> SetUserPoint(Guid userId, int point, string descriptionTransaction = "Initial point.");
    Task<UserPoint> GetUserPoint(Guid userId);
    List<PointTransaction> GetAllPointTransaction();
    Task<bool> DeletePointTransaction(Guid PointTransactionId);
    //Task SetUserPoint(Guid studentId, int? pointPayment);
}