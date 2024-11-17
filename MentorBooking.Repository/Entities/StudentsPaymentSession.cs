using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class StudentsPaymentSession
{
    public Guid SessionId { get; set; }

    public Guid StudentId { get; set; }

    public int PointsChanged { get; set; }

    public DateTime PaidAt { get; set; }

    public virtual MentorSupportSession Session { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
