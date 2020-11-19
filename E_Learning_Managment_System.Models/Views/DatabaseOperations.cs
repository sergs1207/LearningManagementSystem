using E_Learning_Managment_System.Models;
using E_Learning_Managment_System.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace E_Learning_Managment_System.ViewModel
{
    public class DatabaseOperations
    {
        /// <param name="std"></param>
        public void StudentSignUp(Student std)
        {
            DbConnect db = new DbConnect();
            db.Student.Add(std);
            db.SaveChanges();
        }

/// <param name="inst"></param>
public void InstructorSignUp(Instructor inst)
        {
            DbConnect db = new DbConnect();
            db.Instructor.Add(inst);
            db.SaveChanges();
        }
        /// the function that gets the student's record from database when his/her profile is opened
        public Student SearchAndReturnStudent(int p)
        {
            using (var db = new DbConnect())
            {
                return db.Student.Find(p);
            }
        }
        /// this function gets the instructor's record from the database when his/her profile in opened
        public Instructor SearchAndReturnInstructor(int p)
        {
            using (var db = new DbConnect())
            {
                return db.Instructor.Find(p);
                
            }
        }
        /// function used to send name suggestions in messaging module
        public List<AutoCompleteViewModel> Autocomplete(string prefix, int userId, string
        senderType)
        {
            using (var db = new DbConnect())
            {
                if (senderType == "student")
                {
                    var Student = (from student in db.Student
                                   where student.FirstName.StartsWith(prefix)
                                   && student.ID != userId
                                   select new AutoCompleteViewModel
                                   {
                                       recieverType = "student",
                                       senderType = "student",
                                       val = student.ID,
                                       label = student.FirstName + " " + student.LastName
                                   }).ToList();
                    var Teacher = (from teacher in db.Instructor

                where teacher.FirstName.StartsWith(prefix)
                select new AutoCompleteViewModel
                {
                    recieverType = "instructor",
                    senderType = "student",
                    val = teacher.ID,
                    label = teacher.FirstName + " " + teacher.LastName
                }).ToList();
                    return Student.Concat(Teacher).ToList();
                }
                else if (senderType == "instructor")
                {
                    var Student = (from student in db.Student
                                   where student.FirstName.StartsWith(prefix)
                                   select new AutoCompleteViewModel
                                   {
                                       recieverType = "student",
                                       senderType = "instructor",
                                       val = student.ID,
                                       label = student.FirstName + " " + student.LastName
                                   
                                   }).ToList();
                    var Teacher = (from teacher in db.Instructor
                                   where teacher.FirstName.StartsWith(prefix)
                                   && teacher.ID != userId
                                   select new AutoCompleteViewModel
                                   {
                                       recieverType = "instructor",
                                       senderType = "instructor",
                                       val = teacher.ID,
                                       label = teacher.FirstName + " " + teacher.LastName
                                   }).ToList();
                    return Student.Concat(Teacher).ToList();
                }
                return null;
            }
        }
        /// save new message in database
        public void PostMessage(Messages message)
        {
            using (var db = new DbConnect())
                
        {
                if (message.MessageId == default(int))
                {
                    db.Messages.Add(message);
                    db.SaveChanges();
                }
            }
        }
        /// get all messages with a single person
        /// <param name="message"></param>
        /// <returns></returns>
        public List<Messages> GetMessages(Messages message)
        {
            using (var db = new DbConnect())
            {
                if (message.SenderStudentId != null && message.RecieverStudentId != null)
                {
                    var a = db.Messages.Where(t => t.SenderStudentId ==
                    message.SenderStudentId && t.RecieverStudentId ==
                    message.RecieverStudentId).ToList();
                    
                var b = db.Messages.Where(t => t.SenderStudentId ==
                message.RecieverStudentId && t.RecieverStudentId ==
                message.SenderStudentId).ToList();
                    var c = a.Concat(b).OrderByDescending(t => t.DateTime).ToList();
                    return c;
                }
                else if (message.SenderStudentId != null && message.RecieverInstructorId !=
                null)
                {
                    var a = db.Messages.Where(t => t.SenderStudentId ==
                    message.SenderStudentId && t.RecieverInstructorId ==
                    message.RecieverInstructorId).ToList();
                    var b = db.Messages.Where(t => t.SenderInstructorId ==
                    message.RecieverInstructorId && t.RecieverStudentId ==
                    message.SenderStudentId).ToList();
                    var c = a.Concat(b).OrderByDescending(t => t.DateTime).ToList();
                    return c;
                }
                else if (message.SenderInstructorId != null && message.RecieverInstructorId
                != null)
                {
                    
                var a = db.Messages.Where(t => t.SenderInstructorId ==
                message.SenderInstructorId && t.RecieverInstructorId ==
                message.RecieverInstructorId).ToList();
                    var b = db.Messages.Where(t => t.SenderInstructorId ==
                    message.RecieverInstructorId && t.RecieverInstructorId ==
                    message.SenderInstructorId).ToList();
                    var c = a.Concat(b).OrderByDescending(t => t.DateTime).ToList();
                    return c;
                }
                else if (message.SenderInstructorId != null && message.RecieverStudentId !=
                null)
                {
                    var a = db.Messages.Where(t => t.SenderInstructorId ==
                    message.SenderInstructorId && t.RecieverStudentId ==
                    message.RecieverStudentId).ToList();
                    var b = db.Messages.Where(t => t.SenderStudentId ==
                    message.RecieverStudentId && t.RecieverInstructorId ==
                    message.SenderInstructorId).ToList();
                    var c = a.Concat(b).OrderByDescending(t => t.DateTime).ToList();
                    return c;
                }
                return null;
            }
            
        }
        /// get all latest messages in the chat
        public List<Messages> GetAllMessages(Messages message)
        {
            using (var db = new DbConnect())
            {
                if (message.SenderStudentId != null && message.RecieverStudentId != null)
                {
                    var myId = message.SenderStudentId;
                    var meSending = db.Messages.Where(x => x.SenderStudentId ==
                    myId).ToList();
                    var sendingMsgToStudent = meSending.Where(x => x.RecieverStudentId !=
                    null).GroupBy(x => x.RecieverStudentId).Select(x => x.Last());
                    var sendingMsgToInstructor = meSending.Where(x =>
                    x.RecieverInstructorId != null).GroupBy(x => x.RecieverInstructorId).Select(x =>
                    x.Last());
                    var meRecieving = db.Messages.Where(x => x.RecieverStudentId ==
                    myId).ToList();
                    var recievingMsgFromStudent = meRecieving.Where(x =>
                    x.SenderStudentId != null).GroupBy(x => x.SenderStudentId).Select(x => x.Last());
                    
                var recievingMsgFromInstructor = meRecieving.Where(x =>
                x.SenderInstructorId != null).GroupBy(x => x.SenderInstructorId).Select(x => x.Last());
                    var chatWithStudent =
                    sendingMsgToStudent.Concat(recievingMsgFromStudent).OrderBy(t =>
                    t.DateTime).ToList();
                    var chatWithInstructor =
                    sendingMsgToInstructor.Concat(recievingMsgFromInstructor).OrderBy(t =>
                    t.DateTime).ToList();
                    List<Messages> deleteRows = new List<Messages>();
                    foreach (var s in chatWithStudent)
                    {
                        if (myId != s.RecieverStudentId)
                        {
                            var p = s.RecieverStudentId;
                            var twoRows = chatWithStudent.Where(x => x.SenderStudentId == p ||
                            x.RecieverStudentId == p);
                            int i = 1;
                            var firstRow = new Messages();
                            
                        var secondRow = new Messages();
                            foreach (var t in twoRows)
                            {
                                if (i == 1)
                                {
                                    firstRow = t;
                                }
                                else if (i == 2)
                                {
                                    secondRow = t;
                                }
                                i++;
                            }
                            if (firstRow != null && secondRow != null)
                            {
                                if (firstRow.DateTime < secondRow.DateTime)
                                {
                                    deleteRows.Add(firstRow);
                                }
                                else if (firstRow.DateTime > secondRow.DateTime)
                                {
                                    deleteRows.Add(secondRow);
                                }
                                
                            }
                        }
                    }
                    foreach (var s in chatWithInstructor)
                    {
                        var p = 0;
                        if (s.SenderInstructorId != null)
                        {
                            p = s.SenderInstructorId ?? default(int);
                        }
                        else if (s.RecieverInstructorId != null)
                        {
                            p = s.RecieverInstructorId ?? default(int);
                        }
                        var twoRows = chatWithInstructor.Where(x => x.SenderInstructorId == p
                        || x.RecieverInstructorId == p);
                        int i = 1;
                        var firstRow = new Messages();
                        var secondRow = new Messages();
                        foreach (var t in twoRows)
                        {
                            
                        if (i == 1)
                            {
                                firstRow = t;
                            }
                            else if (i == 2)
                            {
                                secondRow = t;
                            }
                            i++;
                        }
                        if (firstRow != null && secondRow != null)
                        {
                            if (firstRow.DateTime > secondRow.DateTime)
                            {
                                deleteRows.Add(secondRow);
                            }
                            else if (firstRow.DateTime < secondRow.DateTime)
                            {
                                deleteRows.Add(firstRow);
                            }
                        }
                    }
                    
                var allLatestMessages =
                chatWithStudent.Concat(chatWithInstructor).ToList();
                    foreach (var d in deleteRows)
                    {
                        try
                        {
                            var del = allLatestMessages.First(x => x.MessageId == d.MessageId);
                            allLatestMessages.Remove(del);
                            db.SaveChanges();
                        }
                        catch { }
                    }
                    return allLatestMessages.OrderByDescending(t => t.DateTime).ToList();
                }
                else if (message.SenderInstructorId != null && message.RecieverInstructorId
                != null)
                {
                    var myId = message.SenderInstructorId;
                    var meSending = db.Messages.Where(x => x.SenderInstructorId ==
                    myId).ToList();
                    
                var sendingMsgToInstructor = meSending.Where(x =>
                x.RecieverInstructorId != null).GroupBy(x => x.RecieverInstructorId).Select(x =>
                x.Last());
                    var sendingMsgToStudent = meSending.Where(x => x.RecieverStudentId !=
                    null).GroupBy(x => x.RecieverStudentId).Select(x => x.Last());
                    var meRecieving = db.Messages.Where(x => x.RecieverInstructorId ==
                    myId).ToList();
                    var recievingMsgFromInstructor = meRecieving.Where(x =>
                    x.SenderInstructorId != null).GroupBy(x => x.SenderInstructorId).Select(x => x.Last());
                    var recievingMsgFromStudent = meRecieving.Where(x =>
                    x.SenderStudentId != null).GroupBy(x => x.SenderStudentId).Select(x => x.Last());
                    var chatWithInstructor =
                    sendingMsgToInstructor.Concat(recievingMsgFromInstructor).OrderBy(t =>
                    t.DateTime).ToList();
                    var chatWithStudent =
                    sendingMsgToStudent.Concat(recievingMsgFromStudent).OrderBy(t =>
                    t.DateTime).ToList();
                    List<Messages> deleteRows = new List<Messages>();
                    foreach (var s in chatWithInstructor)
                        
                {
                        if (myId != s.RecieverInstructorId)
                        {
                            var p = s.RecieverInstructorId;
                            var twoRows = chatWithInstructor.Where(x => x.SenderInstructorId ==
                            p || x.RecieverInstructorId == p);
                            int i = 1;
                            var firstRow = new Messages();
                            var secondRow = new Messages();
                            foreach (var t in twoRows)
                            {
                                if (i == 1)
                                {
                                    firstRow = t;
                                }
                                else if (i == 2)
                                {
                                    secondRow = t;
                                }
                                i++;
                            }
                            
                        if (firstRow != null && secondRow != null)
                            {
                                if (firstRow.DateTime < secondRow.DateTime)
                                {
                                    deleteRows.Add(firstRow);
                                }
                                else if (firstRow.DateTime > secondRow.DateTime)
                                {
                                    deleteRows.Add(secondRow);
                                }
                            }
                        }
                    }
                    foreach (var s in chatWithStudent)
                    {
                        var p = 0;
                        if (s.SenderStudentId != null)
                        {
                            p = s.SenderStudentId ?? default(int);
                        }
                        else if (s.RecieverStudentId != null)
                        {
                            
                        p = s.RecieverStudentId ?? default(int);
                        }
                        var twoRows = chatWithStudent.Where(x => x.SenderStudentId == p ||
                        x.RecieverStudentId == p);
                        int i = 1;
                        var firstRow = new Messages();
                        var secondRow = new Messages();
                        foreach (var t in twoRows)
                        {
                            if (i == 1)
                            {
                                firstRow = t;
                            }
                            else if (i == 2)
                            {
                                secondRow = t;
                            }
                            i++;
                        }
                        if (firstRow != null && secondRow != null)
                        {
                            
                        if (firstRow.DateTime > secondRow.DateTime)
                            {
                                deleteRows.Add(secondRow);
                            }
                            else if (firstRow.DateTime < secondRow.DateTime)
                            {
                                deleteRows.Add(firstRow);
                            }
                        }
                    }
                    var allLatestMessages =
                    chatWithInstructor.Concat(chatWithStudent).ToList();
                    foreach (var d in deleteRows)
                    {
                        try
                        {
                            var del = allLatestMessages.First(x => x.MessageId == d.MessageId);
                            allLatestMessages.Remove(del);
                            db.SaveChanges();
                        }
                        catch { }
                        
                    }
                    return allLatestMessages.OrderByDescending(t => t.DateTime).ToList();
                }
                return null;
            }
        }
    }
}