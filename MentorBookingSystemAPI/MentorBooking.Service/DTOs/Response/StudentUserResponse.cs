using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class StudentUserResponse
    {
        public Guid? StudentId { get; set; }
        public string? UserName { get; set;}
        public string? Email {  get; set;}
        public int? countGroup { get; set; }
        public int? PointBalance { get; set; }
        public DateTime? CreateAt { get; set;}
    }
}
