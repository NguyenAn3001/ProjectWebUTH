using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Request
{
    public class LogoutModelRequest
    {
        [Required(ErrorMessage = "Require user id for remove refresh token to log out")]
        public string? UserId { get; set; }
    }

}
