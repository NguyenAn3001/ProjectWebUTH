using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Request
{
    public class MentorSupportSessionRequest
    {
        [Required(ErrorMessage = "Student id is required for booking")]
        public Guid StudentId { get; set; }
        [Required(ErrorMessage = "Mentor id is required for booking")]
        public Guid MentorId { get; set; }
        [Required(ErrorMessage = "SessionCount is required for booking")]
        public byte SessionCount { get; set; }
        [Required(ErrorMessage = "PointPerSession is required for booking")]
        public short PointPerSession { get; set; }
        [Required(ErrorMessage ="Date booking is required for booking")]
        public List<Guid> dateBookings { get; set; }
        [Required(ErrorMessage = "Group id is required for booking")]
        public int GroupId {  get; set; }
    }
}