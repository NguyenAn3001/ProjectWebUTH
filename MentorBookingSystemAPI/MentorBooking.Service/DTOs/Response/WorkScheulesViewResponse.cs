using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class WorkScheulesViewResponse
    {
        public Guid ScheduleId { get; set; }
        public Guid UnAvailableScheduleId { get; set; }
        public Guid SessionId { get; set; }
        public bool IsActive { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
