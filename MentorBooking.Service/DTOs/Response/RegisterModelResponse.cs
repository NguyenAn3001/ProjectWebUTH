using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class RegisterModelResponse
    {
        public Guid UserId { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
}
