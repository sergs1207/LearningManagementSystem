using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Learning_Managment_System.Models
{
    public class Messages
    {
        [Key]
        public int MessageId { get; set; }
        public string MessageBody { get; set; }
        public DateTime Date { get; set; }
    public int? SenderStudentId { get; set; }
    public int? SenderInstructorId { get; set; }
    public int? RecieverStudentId { get; set; }
    public int? RecieverInstructorId { get; set; }
    public string SenderName { get; set; }
    public string RecieverName { get; set; }
    }
}