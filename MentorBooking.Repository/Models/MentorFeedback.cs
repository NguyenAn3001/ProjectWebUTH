using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Models;

public partial class MentorFeedback
{
    public Guid FeedbackId { get; set; }

    public Guid SessionId { get; set; }

    public Guid StudentId { get; set; }

    public Guid MentorId { get; set; }

    public byte Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Mentor Mentor { get; set; } = null!;

    public virtual MentorSupportSession Session { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
