/// home page of student module for LME4U
using E_Learning_Managment_System.Models;
using E_Learning_Managment_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;



namespace E_Learning_Managment_System.Controllers
{
    public class StudentController : Controller
    {
        DatabaseOperations obj = new DatabaseOperations();
        DbConnect db = new DbConnect();
        //variable to store the ids of questions during quiz

        public static List<int> rand_question = new List<int>();
        public static int marks;
        Random random = new Random();

        public ActionResult Index()
        {
            if (Session["StudentID"] != null)
            {
                List<Course> courseList = new List<Course>();
                List<Result> resultList = new List<Result>();
                var studentId = (int)Session["StudentID"];
                try
                {
                    var courses = db.StudentCourseAssignment.Where(x => x.StudentID == studentId).ToList();
                    foreach (var c in courses)
                    {
                        var course = db.Course.First(x => x.CourseID == c.CourseID);
                        courseList.Add(course);
                        try
                        {
                            var quizes = db.Quiz.Where(x => x.CourseID ==
                            course.CourseID).ToList();
                            foreach (var q in quizes)
                            {
                                try

                                {
                                    var results = db.Result.Where(x => x.QuizID == q.ID &&
                                    x.StudentID == studentId).ToList();
                                    foreach (var r in results)
                                    {
                                        resultList.Add(r);
                                    }
                                }
                                catch { }
                            }
                        }
                        catch { }
                    }
                }
                catch { }
                ViewBag.mycourses = courseList;
                ViewBag.results = resultList;
                return View();
            }
            else
            {

                return RedirectToAction("Login", "Student");
            }
        }
        /// Function for downloading files
        /// 
        public ActionResult DownloadsFile()
        {
            if (Session["StudentID"] != null)
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
                return RedirectToAction("Login", "Student");

            }
        }
        /// function for downloading the files
        public FileResult Download(string FileName)
        {
            return new FilePathResult("~/App_Data/Files/" + FileName,
            System.Net.Mime.MediaTypeNames.Application.Octet)
            {
                FileDownloadName = FileName
            };
        }
        /// login for student module
        public ActionResult Login()
        {
            if (Session["StudentID"] != null)
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                return View();

            }
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel std)
        {
            Session.Remove("InstructorID");
            if (ModelState.IsValid)
            {
                try
                {
                    DbConnect dbc = new DbConnect();
                    var s = dbc.Student.First(m => m.Email == std.Email && m.password ==
                    std.password);
                    Session["StudentID"] = s.ID;
                    Session["Name"] = s.FirstName + " " + s.LastName;
                    return RedirectToAction("Index", "Student");
                }
                catch
                {
                    ViewBag.Message = "Invalid Email or Password !";
                    return View();
                }

            }
            return View();
        }
        /// signup page for student module
        public ActionResult SignUp()
        {
            if (Session["StudentID"] != null)
            {
                return RedirectToAction("Login", "Student");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult SignUp(StudentSignUpViewModel std)
        {
            if (ModelState.IsValid)
            {
                Student s = new Student();

                s.FirstName = std.FirstName;
                s.LastName = std.LastName;
                s.Email = std.Email;
                s.password = std.password;
                s.RePassword = std.RePassword;
                obj.StudentSignUp(s);
                return RedirectToAction("Login", "Student");
            }
            //ViewBag.Message = "Something went wrong either you are already registered, or you have entered wrong values";
        return View();
        }
        /// this function checks that if email already exists or not during signup
        [HttpPost]
        public JsonResult CheckEmail(string Email)
        {
            return Json(!db.Student.Any(a => a.Email.Equals(Email)));
        }
        /// funtion for logout
        public ActionResult Logout()
        {

            Session.Abandon();
            return RedirectToAction("Login", "Student");
        }
        /// Function to view the student profile page
        public ActionResult Profile()
        {
            if (Session["StudentID"] != null)
            {
                Student std =
                obj.SearchAndReturnStudent(Convert.ToInt32(Session["StudentID"]));
                ViewBag.password = TempData["Password"];
                return View(std);
            }
            else
            {
                return RedirectToAction("Login", "Student");
            }
        }
        /// page for chatting in student module
        public ActionResult Messages()
        {

            if (Session["StudentID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Student");
            }
        }
        /// this function is used to show all courses in student module
        /// <returns>list of courses</returns>
        public ActionResult ShowAllCourses()
        {
            if (Session["StudentID"] != null)
            {
                //List<Course> semesterCourseList = new List<Course>();
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
                return RedirectToAction("Login", "Student");
            }
        }
        /// this function is used to show those courses to students in which they are
        // registered
/// <returns>list of courses</returns>
public ActionResult ShowMyCourses()
        {
            if (Session["StudentID"] != null)
            {
                //List<Course> semesterCourseList = new List<Course>();
                List<Course> courseList = new List<Course>();
                var studentId = (int)Session["StudentID"];
                if (Session["StudentID"] != null)

                {
                    var myCourses = db.StudentCourseAssignment.Where(x => x.StudentID ==
                    studentId).ToList();
                    foreach (var course in myCourses)
                    {
                        var searchedCourse = db.Course.First(x => x.CourseID ==
                        course.CourseID);
                    }
                }
                ViewBag.Courses = courseList;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Student");
            }
        }

        /// this function is used to register a student in course
        public ActionResult TakeCourse()
        {
            if (Session["StudentID"] != null)
            {
                List<Course> allCourses = new List<Course>();
                List<Course> courses = new List<Course>();
                int studentID = (int)Session["StudentID"];
                allCourses = db.Course.ToList();
                courses = db.Course.ToList();
                var myCourses = db.StudentCourseAssignment.Where(x => x.StudentID ==
                studentID);
                if (myCourses != null)
                {
                    foreach (var i in allCourses)
                    {
                        foreach (var j in myCourses)
                        {
                            if (i.CourseID == j.CourseID)
                            {
                                try

                                {
                                    var c = courses.First(x => x.CourseID == i.CourseID);
                                    courses.Remove(c);
                                }
                                catch { }
                            }
                        }
                    }
                }
                return View(courses);
            }
            else
            {
                return RedirectToAction("Login", "Student");
            }
        }
        [HttpPost]
        public ActionResult TakeCourse(string CourseID)
        {
            StudentCourseAssignment studentCourseAssignment = new
            StudentCourseAssignment();

            studentCourseAssignment.CourseID = CourseID;
            studentCourseAssignment.StudentID = (int)Session["StudentID"];
            db.StudentCourseAssignment.Add(studentCourseAssignment);
            db.SaveChanges();
            return RedirectToAction("ShowMyCourses", "Student");
        }
        /// this shows all the quizes of those subjects in which the student is enrolled
        /// <returns>quizes list</returns>
        public ActionResult StartQuiz()
        {
            if (Session["StudentID"] != null)
            {
                List<Quiz> quizList = new List<Quiz>();
                var studentId = (int)Session["StudentID"];
                var myCourses = db.StudentCourseAssignment.Where(x => x.StudentID ==
                studentId).ToList();
                foreach (var course in myCourses)
                {
                    var searchedCourse = db.Course.First(x => x.CourseID ==
                    course.CourseID);

                    var quizes = db.Quiz.Where(x => x.CourseID ==
                    searchedCourse.CourseID);
                    foreach (var q in quizes)
                    {
                        quizList.Add(q);
                    }
                }
                ViewBag.Quizes = quizList;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Student");
            }
        }
        /// this function is used to conduct the quiz
        /// <param name="QuizID"></param>
        /// <returns>question</returns>
        public ActionResult Quiz(int QuizID)
        {

            if (Session["StudentID"] != null)
            {
                try
                {
                    var quiz = db.Quiz.First(x => x.ID == QuizID);
                    int quizID = quiz.ID;
                    marks = 0;
                    rand_question.Clear();
                    var total_questions = db.QuizQuestions.Where(x => x.QuizID == quiz.ID);
                    var count_total_questions = total_questions.Count();
                    int count_questions = rand_question.Count();
                    int number;
                    do
                    {
                        number = random.Next(1, (count_total_questions + 1));
                    } while (rand_question.Contains(number));
                    rand_question.Add(number);
                    var question = total_questions.OrderBy(a => a.QuestionID).Skip(number -
                    1).Take(1).ToList();

                    foreach (var s in question)
                    {
                        ViewBag.Question = db.Questions.First(x => x.ID == s.QuestionID);
                        var options = db.Options.Where(x => x.QuestionID == s.QuestionID);
                        List<Options> optionList = new List<Options>();
                        foreach (var o in options)
                        {
                            optionList.Add(o);
                        }
                        ViewBag.Options = optionList;
                    }
                    ViewBag.QuizID = quizID;
                }
                catch
                {
                    ViewBag.Question = null;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Student");

            }
        }
        [HttpPost]
        public ActionResult Quiz(int QuizID, int QuestionID, int? SelectedOption)
        {
            var correctOption = db.Options.First(x => x.QuestionID == QuestionID &&
            x.Status == "correct");
            if (SelectedOption.HasValue)
            {
                if (correctOption.ID == SelectedOption)
                {
                    marks = marks + 1;
                }
            }
            var total_questions = db.QuizQuestions.Where(x => x.QuizID == QuizID);
            var count_total_questions = total_questions.Count();
            int count_questions = rand_question.Count();
            if (count_questions < count_total_questions)
            {
                int number;

                do
                {
                    number = random.Next(1, (count_total_questions + 1));
                } while (rand_question.Contains(number));
                rand_question.Add(number);
                int pick = number - 1;
                var question = total_questions.OrderBy(a =>
                a.QuestionID).Skip(pick).Take(1).ToList();
                foreach (var s in question)
                {
                    ViewBag.Question = db.Questions.First(x => x.ID == s.QuestionID);
                    var options = db.Options.Where(x => x.QuestionID == s.QuestionID);
                    List<Options> optionList = new List<Options>();
                    foreach (var o in options)
                    {
                        optionList.Add(o);
                    }
                    ViewBag.Options = optionList;
                }

                ViewBag.QuizID = QuizID;
                ViewBag.finish = "no";
            }
            else
            {
                ViewBag.finish = "yes";
                ViewBag.TotalMarks = count_total_questions;
                ViewBag.ObtainedMarks = marks;
                var studentId = (int)Session["StudentID"];
                var quiz = db.Quiz.First(x => x.ID == QuizID);
                var course = db.Course.First(x => x.CourseID == quiz.CourseID);
                var instructor = db.InstructorCourseAssignment.First(x => x.CourseID ==
                course.CourseID);
                Result result = new Result();
                result.QuizID = QuizID;
                result.StudentID = studentId;
                result.InstructorID = instructor.InstructorID;
                result.CourseID = course.CourseID;
                result.TotalMarks = ViewBag.TotalMarks;
                result.ObtainedMarks = ViewBag.ObtainedMarks;

                db.Result.Add(result);
                db.SaveChanges();
            }
            return View();
        }
        /// this function is used to show quiz result after the quiz is finished
        public ActionResult QuizResult()
        {
            if (Session["StudentID"] != null)
            {
                var studentId = (int)Session["StudentID"];
                var result = db.Result.Where(x => x.StudentID == studentId);
                return View(result);
            }
            else
            {
                return RedirectToAction("Login", "Student");
            }
        }
        /// page to confirm old password

        public ActionResult ChangePassword()
        {
            if (Session["StudentID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Student");
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(string password)
        {
            int studentID = (int)Session["StudentID"];
            if (Session["StudentID"] != null)
            {
                var std = db.Student.First(x => x.ID == studentID);
                if (std.password == password)
                {
                    Session["PasswordCheck"] = "yes";
                    return RedirectToAction("Change", "Student");

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
        /// <summary>
        /// function to change the student's account password
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult Change()
        {
            if (Session["StudentID"] != null)
            {
                var check = (string)Session["PasswordCheck"];
                if (check == "yes")
                {
                    Session.Remove("PasswordCheck");

                    return View();
                }
                return RedirectToAction("ChangePassword", "Student");
            }
            else
            {
                return RedirectToAction("Login", "Student");
            }
        }
        [HttpPost]
        public ActionResult change(PasswordViewModel std)
        {
            int studentID = (int)Session["StudentID"];
            if (Session["StudentID"] != null)
            {
                var s = db.Student.First(x => x.ID == studentID);
                s.password = std.password;
                s.RePassword = std.RePassword;
                db.SaveChanges();
                TempData["Password"] = "Password changed successfully !";

                return RedirectToAction("Profile", "Student");
            }
            return View();
        }
    }
}