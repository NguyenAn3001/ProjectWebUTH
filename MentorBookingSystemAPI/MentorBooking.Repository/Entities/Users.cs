using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Entities
{
    public class Users : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public virtual Mentor Mentor { get; set; }
        public virtual Student Student { get; set; }
        public virtual UserPoint UserPoint { get; set; }
    }
}
