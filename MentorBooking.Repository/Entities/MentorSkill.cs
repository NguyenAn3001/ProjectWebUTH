using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Entities
{
    public class MentorSkill
    {
        public Guid MentorId { get; set; }
        public int SkillId { get; set; }

        public Mentor Mentor { get; set; }
        public Skill Skill { get; set; }
    }
}
