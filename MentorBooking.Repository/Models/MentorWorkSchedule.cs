using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Models;

public partial class MentorWorkSchedule
{
    public Guid ScheduleId { get; set; }

    public Guid SessionId { get; set; }

    public DateOnly WorkDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public byte UnavailableDate { get; set; }

    public virtual MentorSupportSession Session { get; set; } = null!;
}
