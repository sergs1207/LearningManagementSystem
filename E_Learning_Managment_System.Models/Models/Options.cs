using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace E_Learning_Managment_System.Models
{
public class Options
{
[Key]
    public int ID { get; set; }
    [Required(ErrorMessage = "Option is Required !")]
    public string Option { get; set; }
    [Required(ErrorMessage = "Status is Required !")]
    public string Status { get; set; }
    [Required(ErrorMessage = "Question ID is Required !")]
    public int QuestionID { get; set; }
}
}