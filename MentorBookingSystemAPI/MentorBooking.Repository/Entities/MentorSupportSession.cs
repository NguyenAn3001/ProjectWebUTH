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
<<<<<<< HEAD
    public bool SessionConfirm { get; set; } = false;
=======
    public bool SessionConfirm {  get; set; }
>>>>>>> b5703bed503e2988db721df0e5f0f77df6051c9d

    public virtual ProjectGroup Group { get; set; } = null!;

    public virtual GroupFeedback? GroupFeedback { get; set; }

    public virtual Mentor Mentor { get; set; } = null!;

    public virtual ICollection<MentorFeedback> MentorFeedbacks { get; set; } = new List<MentorFeedback>();

    public virtual ICollection<MentorWorkSchedule> MentorWorkSchedules { get; set; } = new List<MentorWorkSchedule>();

    public virtual StudentsPaymentSession? StudentsPaymentSession { get; set; }
}
