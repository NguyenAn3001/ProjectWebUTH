using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Request
{
    public class SettingRoleModelRequest
    {
        [Required(ErrorMessage = "User id for setting role required")]
        public string? UserId { get; set; }
        [Required(ErrorMessage = "Role name for setting role required")]
        public string? RoleName { get; set; }
    }
}
