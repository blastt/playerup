using Market.Model.Models;
using Market.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Market.Web.ViewModels;
using AutoMapper;
using Market.Web.Hubs;
using System.Runtime.Remoting.Contexts;

namespace Market.Web.Controllers
{
    public class MessageController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMessageService _messageService;
        private readonly IOfferService _offerService;
        private readonly IDialogService _dialogService;
        public MessageController(IMessageService messageService, IOfferService offerService, IUserProfileService userProfileService, IDialogService dialogService)
        {
            _messageService = messageService;
            _offerService = offerService;
            _userProfileService = userProfileService;
            _dialogService = dialogService;
        }

        //public IList<Message> GetMessageStory(int messageId, IList<Message> messages)
        //{

        //    var childMes = _db.Messages.Find(p => p.ParentMessageId == messageId).FirstOrDefault();
        //    if (childMes != null)
        //    {
        //        messages.Add(childMes);
        //        GetMessageStory(childMes.Id, messages);
        //    }
        //    return messages;
        //}

        public void DeleteGroup(int? lastMessageId)
        {
            //var childMes = _db.Messages.Find(p => p.ParentMessageId == lastMessageId).LastOrDefault();
            //if (childMes != null && childMes.UserProfileId == User.Identity.GetUserId())
            //{
            //    _db.Messages.Delete(childMes.Id.ToString());
            //    DeleteGroup(childMes.Id);
            //}
        }


        public ActionResult DeleteStory(int? lastMessageId)
        {
            if(lastMessageId != null)
            {
                DeleteGroup(lastMessageId);

                _messageService.DeleteMessage(lastMessageId.Value);
                _messageService.SaveMessage();
                return RedirectToAction("Inbox");
            }
            return HttpNotFound();
        }

        public ActionResult Story(string userId, int? id, int? page)
        {

            //var messageFromUser = _db.Messages.Find(m => m.UserProfileId == User.Identity.GetUserId() && m.SenderId == user).FirstOrDefault();
            //if (id != null)
            //{
            //    var message = _db.Messages.Find(m => m.UserProfileId == User.Identity.GetUserId() && m.Id == id).FirstOrDefault();

            //}

            //var messageToUser = _db.Messages.Find(m => m.SenderId == User.Identity.GetUserId() && m.UserProfileId == user).FirstOrDefault();

            //var user = _db.UserProfiles.Get(userId);
            //if (user.Messages.Any(m => m.SenderId == User.Identity.GetUserId()) || _db.UserProfiles.Get(User.Identity.GetUserId()).Messages.Any(m => m.SenderId == user.Id))
            //{
            //    // Message which saved on sender side
            //    var lastMessageFromUser = user.Messages.FirstOrDefault(m => m.ReceiverId == User.Identity.GetUserId());
            //    // Message which saved on receiver side
            //    var lastMessageFromUserCopy = _db.UserProfiles.Get(User.Identity.GetUserId()).Messages.FirstOrDefault(m => m.ReceiverId == User.Identity.GetUserId());
            //    // Message which saved on sender side
            //    var lastMessageToUser = user.Messages.FirstOrDefault(m => m.SenderId == User.Identity.GetUserId());
            //    // Message which saved on receiver side
            //    var lastMessageToUserCopy = _db.UserProfiles.Get(User.Identity.GetUserId()).Messages.FirstOrDefault(m => m.SenderId == User.Identity.GetUserId());

            //    IList<Message> messageStory = new List<Message>();
            //    if (lastMessageFromUserCopy != null && lastMessageToUserCopy != null)
            //    {
            //        if (DateTime.Compare(lastMessageToUserCopy.CreatedDate, lastMessageFromUserCopy.CreatedDate) > 0)
            //        {
            //            messageStory.Add(lastMessageFromUserCopy);
            //            messageStory = GetLastMessage(lastMessageFromUserCopy.Id, messageStory);
            //        }
            //        else
            //        {
            //            messageStory.Add(lastMessageToUserCopy);
            //            messageStory = GetLastMessage(lastMessageToUserCopy.Id, messageStory);
            //        }
            //    }
            //    else if (lastMessageFromUserCopy != null)
            //    {
            //        messageStory.Add(lastMessageFromUserCopy);
            //        messageStory = GetLastMessage(lastMessageFromUserCopy.Id, messageStory);
            //    }
            //    else if (lastMessageToUserCopy != null)
            //    {
            //        messageStory.Add(lastMessageToUserCopy);
            //        messageStory = GetLastMessage(lastMessageToUserCopy.Id, messageStory);
            //    }
            //    foreach (var message in messageStory)
            //    {
            //        if (message.UserProfileId != User.Identity.GetUserId())
            //            message.IsViewed = true;
            //    }
            //    _db.Save();
            //    int pageSize = 5;
            //    int pageNumber = (page ?? 1);
            //    MessageListViewModel model = new MessageListViewModel
            //    {
            //        Messages = messageStory

            //    };
            //    ViewData["UserId"] = user;
            //    return View(model);
            //}
            return HttpNotFound("User access error!");
        }
        // GET: Message
        public ActionResult Inbox(string sortOrder, string currentFilter, string searchString, string senderName, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            //ViewBag.FilterParam = String.IsNullOrEmpty(filter) ? "all" : "";
            ViewData["SenderName"] = senderName;
            ViewData["SearchString"] = searchString;
            ViewData["CurrentFilter"] = currentFilter;
            var currentUser = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            var dialogs = _dialogService.GetDialogs().Where(d => d.Users.Any(u => u.Id == currentUser.Id));

            //var messages = _db.Messages.Find(m => m.ReceiverId == User.Identity.GetUserId());

            if (!String.IsNullOrEmpty(searchString))
            {
                //dialogs = dialogs.Where(s => s.MessageBody.Contains(searchString) && s.ReceiverId == User.Identity.GetUserId());
            }
            if (!String.IsNullOrEmpty(senderName))
            {
                //var userProfile = _userProfileService.GetUserProfiles().Where(m => m.ApplicationUser.UserName == senderName).FirstOrDefault();
                //if (userProfile != null)
                //{
                //    messages = messages.Where(s => s.SenderId == userProfile.Id && s.ReceiverId == User.Identity.GetUserId());
                //}
                //else
                //{
                //    messages = Array.Empty<Message>();
                //}

            }
            
            switch (currentFilter)
            {
                case "read":
                    IList<Dialog> readDialogs = new List<Dialog>();
                    foreach (var dialog in dialogs)
                    {
                        if (dialog.Messages.Any(m => m.ToViewed))
                        {
                            readDialogs.Add(dialog);
                        }
                        
                    }
                    dialogs = readDialogs;
                    break;
                case "unread":
                    IList<Dialog> unreadDialogs = new List<Dialog>();
                    foreach (var dialog in dialogs)
                    {
                        if (dialog.Messages.Any(m => !m.ToViewed))
                        {
                            unreadDialogs.Add(dialog);
                        }

                    }
                    dialogs = unreadDialogs;
                    break;

                default:
                    break;
            }
            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        //messages = messages.OrderByDescending(s => s.SenderName);
            //        break;
            //    case "Date":
            //        messages = messages.OrderBy(s => s.CreatedDate);
            //        break;
            //    case "date_desc":
            //        messages = messages.OrderByDescending(s => s.CreatedDate);
            //        break;

            //    default:  // Name ascending 
            //        messages = messages.OrderByDescending(s => s.CreatedDate);
            //        break;
            //}

            DialogListViewModel model = new DialogListViewModel()
            {                
                Dialogs = Mapper.Map<IEnumerable<Dialog>, IEnumerable<DialogViewModel>>(dialogs)

                
            };
            
            foreach (var d in model.Dialogs)
            {
                var otherUser = d.Users.FirstOrDefault(u => u.Id != User.Identity.GetUserId());
                d.otherUserId = otherUser.Id;
                d.otherUserName = otherUser.Name;
                d.CountOfNewMessages = d.Messages.Where(m => !m.ToViewed && m.SenderId != currentUser.Id).Count();
                
            }
            return View(model);
        }

        public ActionResult Unread(string sortOrder, string currentFilter, string searchString, string senderName, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            //ViewBag.FilterParam = String.IsNullOrEmpty(filter) ? "all" : "";
            ViewData["SenderName"] = senderName;
            ViewData["SearchString"] = searchString;
            ViewData["CurrentFilter"] = currentFilter;
            var currentUser = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            var dialogs = new List<DialogViewModel>();

            //var messages = _db.Messages.Find(m => m.ReceiverId == User.Identity.GetUserId());

            if (!String.IsNullOrEmpty(searchString))
            {
                //dialogs = dialogs.Where(s => s.MessageBody.Contains(searchString) && s.ReceiverId == User.Identity.GetUserId());
            }
            if (!String.IsNullOrEmpty(senderName))
            {
                //var userProfile = _userProfileService.GetUserProfiles().Where(m => m.ApplicationUser.UserName == senderName).FirstOrDefault();
                //if (userProfile != null)
                //{
                //    messages = messages.Where(s => s.SenderId == userProfile.Id && s.ReceiverId == User.Identity.GetUserId());
                //}
                //else
                //{
                //    messages = Array.Empty<Message>();
                //}

            }

            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        //messages = messages.OrderByDescending(s => s.SenderName);
            //        break;
            //    case "Date":
            //        messages = messages.OrderBy(s => s.CreatedDate);
            //        break;
            //    case "date_desc":
            //        messages = messages.OrderByDescending(s => s.CreatedDate);
            //        break;

            //    default:  // Name ascending 
            //        messages = messages.OrderByDescending(s => s.CreatedDate);
            //        break;
            //}

            DialogListViewModel model = new DialogListViewModel()
            {
                Dialogs = Mapper.Map<IEnumerable<Dialog>, IEnumerable<DialogViewModel>>(_dialogService.GetDialogs())


            };
            
            foreach (var d in model.Dialogs)
            {
                var otherUser = d.Users.FirstOrDefault(u => u.Id != User.Identity.GetUserId());
                d.otherUserId = otherUser.Id;
                d.otherUserName = otherUser.Name;
                d.CountOfNewMessages = d.Messages.Where(m => !m.ToViewed && m.SenderId != currentUser.Id).Count();
                if (d.CountOfNewMessages != 0)
                {
                    dialogs.Add(d);
                }
            }
            model.Dialogs = dialogs;
            return View(model);
        }


        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var user = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
                if (user != null)
                {

                    var message = _messageService.GetMessage(id.Value);
                    if (message.ReceiverId == User.Identity.GetUserId() || message.SenderId == User.Identity.GetUserId())
                    {
                        if (message.ReceiverId == User.Identity.GetUserId())
                        {
                            message.ReceiverDeleted = true;
                        }
                        if (message.SenderId == User.Identity.GetUserId())
                        {
                            message.SenderDeleted = true;
                        }
                        _messageService.SaveMessage();
                        return RedirectToAction("Inbox");
                    }
                    else
                    {
                        return HttpNotFound("Message not found");
                    }


                }
            }
            return HttpNotFound("Delete Error!");
        }

        public JsonResult DeleteAjax(int? id)
        {
            if (id != null)
            {
                var user = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
                if (user != null)
                {

                    var message = _messageService.GetMessage(id.Value);
                    if (message.ReceiverId == User.Identity.GetUserId() || message.SenderId == User.Identity.GetUserId())
                    {
                        if (message.ReceiverId == User.Identity.GetUserId())
                        {
                            message.ReceiverDeleted = true;
                        }
                        if (message.SenderId == User.Identity.GetUserId())
                        {
                            message.SenderDeleted = true;
                        }
                        _messageService.SaveMessage();
                        return Json(new { Success = true });
                    }



                }
            }
            return Json(new { Success = false });

        }

        public JsonResult SetMessagesViewed(int? dialogId)
        {
            if (dialogId != null)
            {
                var dialog = _dialogService.GetDialog(dialogId.Value);
                var dialogMessagessAll = dialog.Messages;
                if (dialogMessagessAll != null)
                {
                    var messages = dialog.Messages.Where(m => m.SenderId != User.Identity.GetUserId() && m.ToViewed == false);
                    foreach (var message in messages)
                    {
                        message.ToViewed = true;
                    }
                }
                
            }
            var user = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            int newDialogsCount = 0;

            foreach (var d in user.Dialogs)
            {
                if (d.Messages.Any(m => !m.ToViewed && m.SenderId != User.Identity.GetUserId()))
                {
                    newDialogsCount++;
                }
            }
            UpdateMessage(newDialogsCount, user.Name);
            _messageService.SaveMessage();
            return Json(new { Success = false });
        }



        // GET: Message/Create
        public ActionResult New(int? id) // orderId
        {
            string offerId = id == null ? "" : id.ToString();
            ViewData["OrderId"] = offerId;
            //ViewBag.UserProfileId = new SelectList(_db.UserProfiles.GetAll(), "Id", "Discription");
            return View();
        }

        // POST: Message/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult New(MessageViewModel model)
        {
            
            if (model.MessageBody.Trim() == "")
            {
                return Json(new { });
            }
            if (ModelState.IsValid)
            {
                // Поменять(Не проверять на наличие оффера)                
                var user = _userProfileService.GetUserProfileById(model.ReceiverId);
                var currentUser = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
                if (user != null)
                {
                    
                    Message message = Mapper.Map<MessageViewModel, Message>(model);
                    message.FromViewed = true;
                    message.SenderId = User.Identity.GetUserId();
                    message.CreatedDate = DateTime.Now;
                    var up = _userProfileService.GetUserProfiles();
                    var dialog = user.Dialogs.FirstOrDefault(d => d.Users.Any(u => u.Id == currentUser.Id));
                    if (dialog == null)
                    {
                        IList<UserProfile> dialogUsers = new List<UserProfile>();
                        dialogUsers.Add(currentUser);
                        dialogUsers.Add(user);
                        dialog = new Dialog()
                        {
                            Users = dialogUsers
                        };
                        
                        _dialogService.CreateDialog(dialog);
                        AddDialog(user.Name, currentUser.Name, user.Id, user.Name, 1, "", "123", dialog.Id);
                    }
                    
                    dialog.Messages.Add(message);
                    
                    _messageService.SaveMessage();
                    int newDialogsCount = 0;

                    //d.CountOfNewMessages = d.Messages.Where(m => !m.ToViewed && m.SenderId != currentUser.Id).Count();
                    newDialogsCount = _dialogService.UnreadDialogsForUserCount(user.Id);
                    
                    foreach (var d in user.Dialogs)
                    {
                        int messageInDialogCount = 0;
                        messageInDialogCount = _dialogService.UnreadMessagesInDialogCount(d);
                        var lastMessage = d.Messages.LastOrDefault();
                        if (lastMessage != null)
                        {
                            var companion = d.Users.FirstOrDefault(u => u.Id == currentUser.Id);
                            if (companion != null)
                            {
                                UpdateMessageInDialog(messageInDialogCount, lastMessage.MessageBody, lastMessage.CreatedDate.ToShortDateString(), d.Id, user.Name, companion.Id, companion.Name);
                            }
                            
                        }
                        
                    }
                    
                    UpdateMessage(newDialogsCount, user.Name);
                    
                    AddMessage(currentUser.Id,user.Name, currentUser.Name, message.MessageBody, message.CreatedDate.ToString());
                    return Json(new {  });
                }
                return Json("Error");
            }

            return Json("Error");
        }

        

        private void UpdateMessage(int messagesCounter, string userId)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            // отправляем сообщение
            context.Clients.User(userId).updateMessage(messagesCounter);
            
            //context.Clients. User(User.Identity.GetUserId()).updateMessage(messagesCounter);
        }

        private void UpdateMessageInDialog(int messagesCounter, string lastMessage, string date, int dialogId , string userName, string companionId, string companionName)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            // отправляем сообщение
            context.Clients.User(userName).updateMessageInDialog(userName, companionId, companionName, messagesCounter,lastMessage,date, dialogId);

            //context.Clients. User(User.Identity.GetUserId()).updateMessage(messagesCounter);
        }

        private void AddMessage(string senderId,string receiverName, string senderName, string messageBody, string date)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            // отправляем сообщение
            //context.Clients.All.addMessage(senderId, receiverId, senderName, messageBody, date);
            context.Clients.User(senderName).addMessage(senderId, receiverName, senderName, messageBody, date);
            context.Clients.User(receiverName).addMessage(senderId, receiverName, senderName, messageBody, date);
            //context.Clients. User(User.Identity.GetUserId()).updateMessage(messagesCounter);
        }

        private void AddDialog(string receiverName, string senderName, string companionId,
            string companionName,int counter, string messageBody, string date, int dialogId)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            // отправляем сообщение
            //context.Clients.All.addMessage(senderId, receiverId, senderName, messageBody, date);
            context.Clients.User(senderName).addDialog(companionId, companionName, counter, messageBody, date, dialogId);
            context.Clients.User(receiverName).addDialog(companionId, companionName, counter, messageBody, date , dialogId);
            //context.Clients. User(User.Identity.GetUserId()).updateMessage(messagesCounter);
        }

        public JsonResult GetMessagessCount()
        {
            string currentUserId = User.Identity.GetUserId();
            int dialogsCount = _dialogService.UnreadDialogsForUserCount(currentUserId);
            

            return Json(dialogsCount);
        }


  
    }
}