﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentorBooking.Repository.Entities;

public partial class ProjectGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid GroupId { get; set; }

    public string? GroupName { get; set; }

    public string? Topic { get; set; }

    public Guid CreateBy { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual ICollection<GroupFeedback> GroupFeedbacks { get; set; } = new List<GroupFeedback>();

    public virtual ICollection<MentorSupportSession> MentorSupportSessions { get; set; } = new List<MentorSupportSession>();

    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
}
