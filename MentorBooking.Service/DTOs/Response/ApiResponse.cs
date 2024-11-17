using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class ApiResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
