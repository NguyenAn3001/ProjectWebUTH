using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class MentorWorkSchedule
{
    public Guid ScheduleId { get; set; }

    public Guid SessionId { get; set; }

    public bool UnavailableDate { get; set; }
    public Guid ScheduleAvailableId { get; set; }
    public virtual SchedulesAvailable ScheduleAvailable { get; set; }
    public virtual MentorSupportSession Session { get; set; } = null!;
}
