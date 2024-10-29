using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.DTOs.Request
{
    public class StudentCommentRequest
    {
        [Required(ErrorMessage ="Rating can't be blank")]
        public byte Rating { get; set; }

        [Required(ErrorMessage = "Rating can't be blank")]
        public string? Comment { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
