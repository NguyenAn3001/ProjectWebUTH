using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class MentorUserResponse
    {
        public Guid MentorId { get; set; }
        public string UserName {  get; set; }
        public string Password { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
