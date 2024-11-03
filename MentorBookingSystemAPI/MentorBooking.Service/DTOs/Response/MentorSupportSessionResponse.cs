using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class MentorSupportSessionResponse
    {
        public Guid StudentId { get; set; }
        public int GroupId {  get; set; }
        public byte SessionCount { get; set; }
        public short PointPerSession { get; set; }
        public int TotalPoint {  get; set; }
    }
}
