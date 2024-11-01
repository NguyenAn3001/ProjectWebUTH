using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class ProjectProgress
{
    public Guid ProgressId { get; set; }

    public Guid GroupId { get; set; }

    public string? Description { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ProjectGroup Group { get; set; } = null!;
}
