using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Models;

public partial class UserPoint
{
    public Guid UserId { get; set; }

    public int? PointsBalance { get; set; }

    public virtual ICollection<PointTransaction> PointTransactions { get; set; } = new List<PointTransaction>();
    public virtual ApplicationUser User { get; set; }
}
