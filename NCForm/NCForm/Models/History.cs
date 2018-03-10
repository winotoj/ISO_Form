using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NCForm.Models
{
    public class History
    {
        public int ID { get; set; }
        [Required, MaxLength(2500), Display(Name = "Action", Description = "Action")]
        public string Message { get; set; }
        public DateTime MsgDate { get; set; }
        public string Creator { get; set; }
        public string FileLoc { get; set; }
        public Issue Issue { get; set; }
        [Required]
        public int IssueId { get; set; }
    }
}