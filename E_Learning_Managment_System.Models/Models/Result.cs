using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace E_Learning_Managment_System.Models
{
public class Result
{
[Key]
    public int ID { get; set; }

[Required(ErrorMessage = "Quiz ID is Required !")]
    public int QuizID { get; set; }
    [Required(ErrorMessage = "Student ID is Required !")]
    public int StudentID { get; set; }
    [Required(ErrorMessage = "Instructor ID is Required !")]
    public int InstructorID { get; set; }
    [Required(ErrorMessage = "Course ID is Required !")]
    public string CourseID { get; set; }
    [Required(ErrorMessage = "Total Marks are Required !")]
    public int TotalMarks { get; set; }
    [Required(ErrorMessage = "Obtained Marks are Required !")]
    public int ObtainedMarks { get; set; }
}
}