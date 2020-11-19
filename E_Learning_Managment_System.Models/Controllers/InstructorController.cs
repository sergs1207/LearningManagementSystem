using E_Learning_Managment_System.Models;

using E_Learning_Managment_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace E_Learning_Managment_System.Controllers
{
    public class InstructorController : Controller
    {
        DatabaseOperations obj = new DatabaseOperations();
        DbConnect db = new DbConnect();

public ActionResult Index()
        {
            if (Session["InstructorID"] != null)
            {
                List<Course> courseList = new List<Course>();
                var instructorId = (int)Session["InstructorID"];
                var courses = db.InstructorCourseAssignment.Where(x => x.InstructorID ==
                instructorId).ToList();
                foreach (var c in courses)
                {
                    var course = db.Course.First(x => x.CourseID == c.CourseID);
                    courseList.Add(course);
                }
                ViewBag.mycourses = courseList;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Instructor");
            }
        }

public ActionResult Profile()
        {
            if (Session["InstructorID"] != null)
            {
                Instructor instructor =
                obj.SearchAndReturnInstructor(Convert.ToInt32(Session["InstructorID"]));
                ViewBag.password = TempData["Password"];
                return View(instructor);
            }
            else
            {
                return RedirectToAction("Login", "Instructor");
            }
        }
        /// Function to upload files
        public ActionResult UploadFile()
        {
            if (Session["InstructorID"] != null)
            {
                return View();
            }
            else
                
        {
                return RedirectToAction("Login", "Instructor");
            }
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/Files"), fileName);
                file.SaveAs(path);
            }
            ViewBag.FileAlert = "Your file has been added successfully !";
            return View();
        }
        /// <summary>
        /// login page of instructor's module
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
    
{
if (Session["InstructorID"] != null)
{
return RedirectToAction("Index", "Instructor");
    }
else
{
return View();
}
}
[HttpPost]
public ActionResult Login(LoginViewModel inst)
{
    Session.Remove("StudentID");
    if (ModelState.IsValid)
    {
        try
        {
            var ins = db.Instructor.First(m => m.Email == inst.Email && m.password
            == inst.password);
            Session["InstructorID"] = ins.ID;
           
        Session["Name"] = ins.FirstName + " " + ins.LastName;
            Session["IsStudent"] = false;
            return RedirectToAction("Index");
        }
        catch
        {
            ViewBag.Message = "Invalid Email or Password !";
            return View();
        }
    }
    return View();
}
/// Function for downloading the files
public ActionResult DownloadsFile()
{
    if (Session["InstructorID"] != null)
    {
        var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/Files"));
        System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");
        List<string> items = new List<string>();
       
    foreach (var file in fileNames)
        {
            items.Add(file.Name);
        }
        return View(items);
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}
/// the function that actually downloads file
public FileResult Download(string FileName)
{
    return new FilePathResult("~/App_Data/Files/" + FileName,
    System.Net.Mime.MediaTypeNames.Application.Octet)
    {
        FileDownloadName = FileName
    };
}

/// signup function for instructor's module
public ActionResult SignUp()
{
    if (Session["InstructorID"] != null)
    {
        return RedirectToAction("Login", "Instructor");
    }
    else
    {
        return View();
    }
}
[HttpPost]
public ActionResult SignUp(InstructorSignUpViewModel inst)
{
    if (ModelState.IsValid)
    {
        Instructor i = new Instructor();
        i.FirstName = inst.FirstName;
        i.LastName = inst.LastName;
        i.Email = inst.Email;
        i.password = inst.password;
        
    i.Repassword = inst.Repassword;
        obj.InstructorSignUp(i);
        return RedirectToAction("Login", "Instructor");
    }
    //ViewBag.Message = "Something went wrong either you are already registered,
    // or you have entered wrong values";
return View();
}
/// this function checks that if email already exists or not during signup
[HttpPost]
public JsonResult CheckExistingEmail(string Email)
{
    return Json(!db.Instructor.Any(a => a.Email.Equals(Email)));
}
/// function used to logout instructor from his account
public ActionResult Logout()
{
    Session.Abandon();
    return RedirectToAction("Login", "Instructor");
}

/// Function to exchange messages in the chat
public ActionResult Messages()
{
    if (Session["InstructorID"] != null)
    {
        return View();
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}
/// show all the courses from database
public ActionResult ShowAllCourses()
{
    if (Session["InstructorID"] != null)
    {
        List<Course> courseList = new List<Course>();
        var allCourses = db.Course.ToList();
        foreach (var course in allCourses)
        {
           
        courseList.Add(course);
        }
        ViewBag.Courses = courseList;
        return View();
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}
/// <summary>
/// shows the courses that are assigned to the instructor
/// </summary>
/// <returns></returns>
public ActionResult ShowMyCourses()
{
    if (Session["InstructorID"] != null)
    {
        List<Course> courseList = new List<Course>();
        var instructorId = (int)Session["InstructorID"];
        if (Session["InstructorID"] != null)
          
    {
            var myCourses = db.InstructorCourseAssignment.Where(x => x.InstructorID
            == instructorId).ToList();
            foreach (var course in myCourses)
            {
                var searchedCourse = db.Course.First(x => x.CourseID ==
                course.CourseID);
                courseList.Add(searchedCourse);
            }
        }
        ViewBag.Courses = courseList;
        return View();
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}
/// shows the list of quizzes made by instructor

public ActionResult ShowQuizes()
{
    if (Session["InstructorID"] != null)
    {
        List<Quiz> quizesList = new List<Quiz>();
        int instructorID = (int)Session["InstructorID"];
        var instructorCourses = db.InstructorCourseAssignment.Where(x =>
        x.InstructorID == instructorID).ToList();
        if (instructorCourses != null)
        {
            foreach (var course in instructorCourses)
            {
                try
                {
                    var quizes = db.Quiz.Where(x => x.CourseID == course.CourseID);
                    foreach (var quiz in quizes)
                    {
                        quizesList.Add(quiz);
                    }
                }
                catch { }
              
            }
        }
        ViewBag.QuizAlert = TempData["quizalert"];
        return View(quizesList);
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}
/// shows the questions of a specific quiz
public ActionResult ShowQuizQuestions(int QuizID)
{
    if (Session["InstructorID"] != null)
    {
        List<Questions> questionsList = new List<Questions>();
        var questions = db.QuizQuestions.Where(x => x.QuizID == QuizID).ToList();
        foreach (var q in questions)
        {
            var a = db.Questions.First(x => x.ID == q.QuestionID);
            questionsList.Add(a);
          
        }
        return View(questionsList);
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}
/// this function is used to generate quiz
public ActionResult QuizGenerator()
{
    if (Session["InstructorID"] != null)
    {
        List<Course> courseList = new List<Course>();
        int instructorID = (int)Session["InstructorID"];
        var instructorCourses = db.InstructorCourseAssignment.Where(x =>
        x.InstructorID == instructorID).ToList();
        if (instructorCourses != null)
           
    {
            foreach (var course in instructorCourses)
            {
                try
                {
                    var q = db.Questions.First(x => x.CourseID == course.CourseID);
                    if (q != null)
                    {
                        var c = db.Course.First(x => x.CourseID == course.CourseID);
                        courseList.Add(c);
                    }
                }
                catch { }
            }
        }
        return View(courseList);
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}

/// this function is used to select the questions to be given in the quiz
public ActionResult SelectQuestions(string CourseID, string Title)
{
    if (Session["InstructorID"] != null)
    {
        var questions = db.Questions.Where(x => x.CourseID == CourseID).ToList();
        ViewBag.CourseID = CourseID;
        ViewBag.CourseTitle = Title;
        return View(questions);
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}
[HttpPost]
public ActionResult SelectQuestions(int[] QuestionIDs, string CourseID, string
Title)
{
    Quiz quiz = new Quiz();
   
quiz.CourseID = CourseID;
    quiz.Title = Title;
    db.Quiz.Add(quiz);
    db.SaveChanges();
    foreach (var qID in QuestionIDs)
    {
        QuizQuestions quizQuestion = new QuizQuestions();
        quizQuestion.QuestionID = qID;
        quizQuestion.QuizID = quiz.ID;
        db.QuizQuestions.Add(quizQuestion);
        db.SaveChanges();
    }
    TempData["quizalert"] = "Quiz has been created successfully !";
    return RedirectToAction("ShowQuizes", "Instructor");
}
/// using this function, the instructor can add question into the subjects he is assigned
public ActionResult AddQuestions(string CourseID)

{
if (Session["InstructorID"] != null)
{
ViewBag.CourseID = CourseID;
return View();
}
else
{
return RedirectToAction("Login", "Instructor");
}
}
[HttpPost]
public ActionResult AddQuestions(FormCollection fc)
{
    Questions question = new Questions();
    Options option = new Options();
    int check = Int32.Parse(fc["Check"]);
    question.CourseID = fc["CourseID"];
    question.Question = fc["Question"];
 
db.Questions.Add(question);
    db.SaveChanges();
    for (int i = 1; i <= 4; i++)
    {
        option.Option = fc["Option" + i];
        option.QuestionID = question.ID;
        if (i == check)
        {
            option.Status = "correct";
        }
        else
        {
            option.Status = "wrong";
        }
        db.Options.Add(option);
        db.SaveChanges();
    }
    ViewBag.CourseID = fc["CourseID"];
    return View();
  
}
/// Show the course's questions
public ActionResult ShowQuestions(string CourseID)
{
    if (Session["InstructorID"] != null)
    {
        var questions = db.Questions.Where(x => x.CourseID == CourseID).ToList();
        return View(questions);
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}
/// page for confirming the old password
public ActionResult ChangePassword()
{
    if (Session["InstructorID"] != null)
    {
        return View();
       
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
}
[HttpPost]
public ActionResult ChangePassword(string password)
{
    int instructorID = (int)Session["InstructorID"];
    if (Session["InstructorID"] != null)
    {
        var ins = db.Instructor.First(x => x.ID == instructorID);
        if (ins.password == password)
        {
            Session["PasswordCheck"] = "yes";
            return RedirectToAction("Change", "Instructor");
        }
        else
        {
            Session["PasswordCheck"] = "no";
            ViewBag.wrongpassword = "Wrong password !";
           
        return View();
        }
    }
    return View();
}
/// page to change the password
public ActionResult Change()
{
    if (Session["InstructorID"] != null)
    {
        var check = (string)Session["PasswordCheck"];
        if (check == "yes")
        {
            Session.Remove("PasswordCheck");
            return View();
        }
        return RedirectToAction("ChangePassword", "Instructor");
    }
    else
    {
        return RedirectToAction("Login", "Instructor");
    }
   
}
[HttpPost]
public ActionResult change(PasswordViewModel ins)
{
    int instructorID = (int)Session["InstructorID"];
    if (Session["InstructorID"] != null)
    {
        var s = db.Instructor.First(x => x.ID == instructorID);
        s.password = ins.password;
        s.Repassword = ins.RePassword;
        db.SaveChanges();
        TempData["Password"] = "Password changed successfully !";
        return RedirectToAction("Profile", "Instructor");
    }
    return View();
}
}
}
