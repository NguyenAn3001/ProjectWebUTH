using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class MentorSupportSession
{
    public Guid SessionId { get; set; }

    public byte SessionCount { get; set; }

    public short PointsPerSession { get; set; }

    public Guid GroupId { get; set; }

    public Guid MentorId { get; set; }

    public int TotalPoints { get; set; }
    public bool SessionConfirm { get; set; } = false;
    public virtual ProjectGroup Group { get; set; } = null!;

    public virtual GroupFeedback? GroupFeedback { get; set; }

    public virtual Mentor Mentor { get; set; } = null!;

    public virtual ICollection<MentorFeedback> MentorFeedbacks { get; set; } = new List<MentorFeedback>();

    public virtual ICollection<MentorWorkSchedule> MentorWorkSchedules { get; set; } = new List<MentorWorkSchedule>();

    public virtual StudentsPaymentSession? StudentsPaymentSession { get; set; }
    public virtual ICollection<ProjectProgress> ProjectProgresses { get; set; } = new List<ProjectProgress>();
}
