using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.Marshalling;

namespace MentorBooking.Repository.Entities
{
    public class SchedulesAvailables
    {
        [Key]
        public Guid ScheduleAvailableId { get; set; }

        [Required]
        public Guid MentorId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateOnly FreeDay { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeOnly StartTime { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeOnly EndTime { get; set; }
        
        public virtual Mentor Mentor { get; set; }
        public virtual MentorWorkSchedule MentorWorkSchedule { get; set; }
    }
}