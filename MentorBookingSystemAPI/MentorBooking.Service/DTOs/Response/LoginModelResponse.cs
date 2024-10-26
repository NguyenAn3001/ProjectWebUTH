using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Response
{
    public class LoginModelResponse
    {
        public string? Status { get; set; } 
        public string? Message { get; set; }    
        public string? AccessToken { get; set; }   
        public string? RefreshToken { get; set; }
        public Guid? UserId { get; set; }
    }
}
