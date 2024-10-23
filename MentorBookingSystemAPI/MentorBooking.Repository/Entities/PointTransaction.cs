using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class PointTransaction
{
    public Guid TransactionId { get; set; }

    public int? PointsChanged { get; set; }

    public bool TransactionType { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid UserId { get; set; }

    public virtual UserPoint User { get; set; } = null!;
}
