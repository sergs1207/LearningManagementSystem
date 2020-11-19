// This is the source code for different courses.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace E_Learning_Managment_System.Models
{
    public class Course
    {
        [Key]
        public int ID { get; set; }
        public string CourseID { get; set; }
        public string CourseTitle { get; set; }

    }
}