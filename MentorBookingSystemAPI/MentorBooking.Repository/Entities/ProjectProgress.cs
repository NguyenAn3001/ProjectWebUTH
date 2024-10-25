using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class ProjectProgress
{
    public int ProgressId { get; set; }

    public int GroupId { get; set; }

    public string? Description { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual ProjectGroup Group { get; set; } = null!;
}
