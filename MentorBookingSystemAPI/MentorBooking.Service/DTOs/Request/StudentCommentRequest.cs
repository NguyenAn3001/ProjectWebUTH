using MentorBooking.Repository.Entities;
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
        [Required(ErrorMessage = "Mentor Id can't be blank")]
        public Guid MentorId {  get; set; }
        [Required(ErrorMessage = "Session Id can't be blank")]
        public Guid SessionId { get; set; }
        [Required(ErrorMessage ="Rating can't be blank")]
        public byte Rating { get; set; }

        [Required(ErrorMessage = "Comment can't be blank")]
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
