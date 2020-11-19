using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace E_Learning_Managment_System.Models
{
public class Questions
{
[Key]
    public int ID { get; set; }
    [Required(ErrorMessage = "Question is Required !")]
    public string Question { get; set; }
    [Required(ErrorMessage = "Course ID is Required !")]
    public string CourseID { get; set; }

}
}