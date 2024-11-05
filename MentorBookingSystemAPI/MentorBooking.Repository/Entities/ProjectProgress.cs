using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class ProjectProgress
{
    public Guid ProgressId { get; set; }

    public Guid SessionId { get; set; }

    public string? Description { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual MentorSupportSession MentorSupportSession { get; set; } = null!;
}
