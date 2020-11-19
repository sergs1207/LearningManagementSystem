using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace E_Learning_Managment_System.Models
{
public class QuizQuestions
{
[Key]
    public int ID { get; set; }
    [Required(ErrorMessage = "Question ID is Required !")]

public int QuestionID { get; set; }
    [Required(ErrorMessage = "Quiz ID is Required !")]
    public int QuizID { get; set; }
}
}