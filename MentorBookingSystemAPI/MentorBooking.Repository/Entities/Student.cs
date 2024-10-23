using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class Student
{
    public Guid StudentId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual ICollection<MentorFeedback> MentorFeedbacks { get; set; } = new List<MentorFeedback>();

    public virtual StudentGroup? StudentGroup { get; set; }

    public virtual ICollection<StudentsPaymentSession> StudentsPaymentSessions { get; set; } = new List<StudentsPaymentSession>();
    public virtual Users User { get; set; }   
}
