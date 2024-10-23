﻿using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Models;

public partial class Mentor
{
    public Guid MentorId { get; set; }

    public Guid UserId { get; set; }

    public byte ExperienceYears { get; set; }

    public string? MentorDescription { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual ICollection<GroupFeedback> GroupFeedbacks { get; set; } = new List<GroupFeedback>();

    public virtual ICollection<MentorFeedback> MentorFeedbacks { get; set; } = new List<MentorFeedback>();

    public virtual ICollection<MentorSupportSession> MentorSupportSessions { get; set; } = new List<MentorSupportSession>();

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
    public virtual ApplicationUser User { get; set; }   
}
