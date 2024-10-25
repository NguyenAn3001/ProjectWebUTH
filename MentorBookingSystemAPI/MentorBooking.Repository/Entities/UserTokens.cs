using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Entities
{
    public class UserTokens : IdentityUserToken<Guid>
    {
        public DateTime Expired { get; set; }
    }
}
