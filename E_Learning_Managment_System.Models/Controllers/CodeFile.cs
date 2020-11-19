/// home page before login for LME4U
using E_Learning_Managment_System.Models;
using E_Learning_Managment_System.ViewModel;

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace E_Learning_Managment_System.Controllers
{
    public class HomeController : Controller
    {
        DatabaseOperations db = new DatabaseOperations();

        public ActionResult Index()
        {
            return View();
        }
        /// function used to give name suggestions in messaging

        [HttpPost]
        public JsonResult AutocompleteSuggestions(string prefix, string senderType)
        {
            if (Session["StudentID"] != null)
            {
                var userId = Convert.ToInt32(Session["StudentID"]);
                var suggestions = db.Autocomplete(prefix, userId, senderType);
                return Json(suggestions);
            }
            else if (Session["InstructorID"] != null)
            {
                var userId = Convert.ToInt32(Session["InstructorID"]);
                var suggestions = db.Autocomplete(prefix, userId, senderType);
                return Json(suggestions);
            }
            return Json(null);
        }

        /// function used to send the message
        [HttpPost]
        public void SendMessage(MessageViewModel model)
        {
            // Save messages
            var message = MaptoMessage(model);
            db.PostMessage(message);
        }
        /// <summary>
        /// function used to get old messages from the chat with a specific person
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMessages(MessageViewModel model)
        {
            var message = MaptoMessage(model);
            var messages = db.GetMessages(message);
            List<MessageViewModel> messagesList = new List<MessageViewModel>();
            if (messages != null)

            {
                foreach (var item in messages)
                {
                    MessageViewModel messageModel = new MessageViewModel();
                    messageModel.messageBody = item.MessageBody;
                    messageModel.dateTime = DateTimeConverter(item.DateTime);
                    messageModel.name = item.SenderName;
                    messagesList.Add(messageModel);
                }
            }
            return Json(messagesList);
        }
        /// <summary>
        /// function to get latest message from all users
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllMessages(MessageViewModel model)
        {
            var message = MaptoAllMessages(model);

            var messages = db.GetAllMessages(message);
            List<MessageViewModel> messagesList = new List<MessageViewModel>();
            if (messages.Count > 0)
            {
                foreach (var item in messages)
                {
                    var id = 0;
                    var name = "";
                    var userType = "";
                    if (model.senderType == "student")
                    {
                        if (model.id == item.SenderStudentId)
                        {
                            if (item.RecieverStudentId != null)
                            {
                                id = item.RecieverStudentId ?? default(int);
                                name = item.RecieverName;
                                userType = "student";
                            }
                            else if (item.RecieverInstructorId != null)
                            {
                                id = item.RecieverInstructorId ?? default(int);
                                name = item.RecieverName;

                                userType = "instructor";
                            }
                        }
                        else if (model.id == item.RecieverStudentId)
                        {
                            if (item.SenderStudentId != null)
                            {
                                id = item.SenderStudentId ?? default(int);
                                name = item.SenderName;
                                userType = "student";
                            }
                            else if (item.SenderInstructorId != null)
                            {
                                id = item.SenderInstructorId ?? default(int);
                                name = item.SenderName;
                                userType = "instructor";
                            }
                        }
                    }
                    else if (model.senderType == "instructor")
                    {
                        if (model.id == item.SenderInstructorId)
                        {

                            if (item.RecieverInstructorId != null)
                            {
                                id = item.RecieverInstructorId ?? default(int);
                                name = item.RecieverName;
                                userType = "instructor";
                            }
                            else if (item.RecieverStudentId != null)
                            {
                                id = item.RecieverStudentId ?? default(int);
                                name = item.RecieverName;
                                userType = "student";
                            }
                        }
                        else if (model.id == item.RecieverInstructorId)
                        {
                            if (item.SenderInstructorId != null)
                            {
                                id = item.SenderInstructorId ?? default(int);
                                name = item.SenderName;
                                userType = "instructor";
                            }
                            else if (item.SenderStudentId != null)
                            {

                                id = item.SenderStudentId ?? default(int);
                                name = item.SenderName;
                                userType = "student";
                            }
                        }
                    }
                    MessageViewModel messageModel = new MessageViewModel();
                    messageModel.messageBody = item.MessageBody;
                    messageModel.dateTime = DateTimeConverter(item.DateTime);
                    messageModel.id = id;
                    messageModel.name = name;
                    messageModel.recieverType = userType;
                    messageModel.senderType = model.senderType;
                    messagesList.Add(messageModel);
                }
            }
            return Json(messagesList);
        }
        /// function for mapping model for student or for instructor on all messages page
        private Messages MaptoAllMessages(MessageViewModel model)
        {

            Messages message = new Messages();
            if (model.senderType == "student")
            {
                message.SenderStudentId = model.id;
                message.RecieverStudentId = model.id;
            }
            else if (model.senderType == "instructor")
            {
                message.SenderInstructorId = model.id;
                message.RecieverInstructorId = model.id;
            }
            return message;
        }
        /// <summary>
        /// function for mapping model for student or for instructor for messaging with specific user
        private Messages MaptoMessage(MessageViewModel model)
        {
            Messages message = new Messages();
            message.DateTime = DateTime.UtcNow;
            message.MessageBody = model.messageBody;
            message.MessageId = model.messageId;

            if (model.recieverType == "student")
            {
                message.RecieverStudentId = model.id;
            }
            else if (model.recieverType == "instructor")
            {
                message.RecieverInstructorId = model.id;
            }
            if (model.senderType == "student")
            {
                message.SenderStudentId = Convert.ToInt32(Session["StudentID"]);
            }
            else if (model.senderType == "instructor")
            {
                message.SenderInstructorId = Convert.ToInt32(Session["InstructorID"]);
            }
            message.SenderName = Session["Name"].ToString();
            message.RecieverName = model.name;
            return message;
        }
    }
}
