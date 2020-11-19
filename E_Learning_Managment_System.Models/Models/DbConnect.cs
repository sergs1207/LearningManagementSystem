/// This is used for database creation
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
namespace E_Learning_Managment_System.Models
{
    public class DbConnect : DbContext
    {
        public DbConnect()
            :base("DbConnect")
        { }
/// All tables that are going to be included in the database

public DbSet<Student> Student { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<InstructorCourseAssignment> InstructorCourseAssignment { get; set; }
        public DbSet<StudentCourseAssignment> StudentCourseAssignment { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<QuizQuestions> QuizQuestions { get; set; }
    }
}