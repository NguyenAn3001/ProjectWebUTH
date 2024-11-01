using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class StudentGroup
{
    public Guid StudentId { get; set; }

    public Guid GroupId { get; set; }

    public DateTime JoinAt { get; set; }

    public virtual ProjectGroup Group { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
