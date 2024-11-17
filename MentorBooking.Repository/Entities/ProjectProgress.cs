using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class ProjectProgress
{
    public Guid ProgressId { get; set; }

    public Guid SessionId { get; set; }

    public string? Description { get; set; }
    public int ProgressIndex { get; set; } = 0;

    public DateTime UpdateAt { get; set; }

    public virtual MentorSupportSession MentorSupportSession { get; set; } = null!;
}
