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
        [Required(ErrorMessage = "ScheduleAvailableId is required for booking")]
        public Guid ScheduleAvailableId { get; set; }
        [Required(ErrorMessage = "ScheduleId is required for booking")]
        public Guid ScheduleId { get; set; }
        [DataType(DataType.Date)]
        public DateOnly FreeDay { get; set; }
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }
    }
}
