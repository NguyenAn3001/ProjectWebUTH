using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class GroupFeedback
{
    public Guid FeedbackId { get; set; }

    public Guid SessionId { get; set; }

    public Guid GroupId { get; set; }

    public Guid MentorId { get; set; }

    public byte Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual ProjectGroup Group { get; set; } = null!;

    public virtual Mentor Mentor { get; set; } = null!;

    public virtual MentorSupportSession Session { get; set; } = null!;
}
