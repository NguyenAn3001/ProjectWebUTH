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
        public int CountSession { get; set; }
        public List<string>? Skills { get; set; } = new List<string>();
        public int Ratings { get; set; }
        public DateTime CreateAt { get; set; }
       

    }
}
