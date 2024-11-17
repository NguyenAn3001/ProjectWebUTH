using System;
using System.Collections.Generic;

namespace MentorBooking.Repository.Entities;

public partial class Skill
{
    public int SkillId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MentorSkill> MentorSkills { get; set; } = new List<MentorSkill>();
}
