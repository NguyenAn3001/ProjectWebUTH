using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }

        public int? PointsChanged { get; set; }

        public bool TransactionType { get; set; }

        public string? Description { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid UserId { get; set; }
        public string? Name {  get; set; }
        public string? Email { get; set; }
        public string? Role {  get; set; }
    }
}
