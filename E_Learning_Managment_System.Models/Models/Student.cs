using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace E_Learning_Managment_System.Models
{
public class Student
{
[Key]
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string password { get; set; }
    public string RePassword { get; set; }
}
}
