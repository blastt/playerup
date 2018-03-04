using Market.Model.Models;
using Market.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    public class MessageController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
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

        public ActionResult Info(int? messageId)
        {
            if (messageId != null)
            {
                if(messageId != null)
                {
                    Message message = _messageService.GetMessage(messageId.Value);
                    
                }
                
                if (message != null && (message.ReceiverId == User.Identity.GetUserId() || message.SenderId == User.Identity.GetUserId()))
                {
                    _messageService.SetMessageViewed(messageId.Value);
                    MessageViewModel model = new MessageViewModel()
                    {
                        Id = message.Id,
                        CreatedDate = message.CreatedDate,
                        IsViewed = message.IsViewed,
                        MessageBody = message.MessageBody,
                        SenderId = message.SenderId,
                        Subject = message.Subject

                    };
                    var offer = _db.Offers.Get(message.Subject);
                    if (offer != null)
                    {
                        ViewData["OfferHeader"] = offer.Header;
                        ViewData["SenderName"] = _db.Users.Get(message.SenderId).UserName;
                        ViewData["ReceiverName"] = _db.Users.Get(message.ReceiverId).UserName;
                    }

                    return View(model);


                }
            }
            return HttpNotFound("Info error!");
        }

        public ActionResult DeleteStory(int? lastMessageId)
        {
            DeleteGroup(lastMessageId);
            _db.Messages.Delete(lastMessageId.ToString());
            _db.Save();
            return RedirectToAction("Inbox");
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
            var messages = _db.Messages.Find(m => m.ReceiverId == User.Identity.GetUserId() && !m.ReceiverDeleted);

            //var messages = _db.Messages.Find(m => m.ReceiverId == User.Identity.GetUserId());

            if (!String.IsNullOrEmpty(searchString))
            {
                messages = messages.Where(s => s.MessageBody.Contains(searchString) && s.ReceiverId == User.Identity.GetUserId());
            }
            if (!String.IsNullOrEmpty(senderName))
            {
                var userProfile = _db.UserProfiles.Find(m => m.ApplicationUser.UserName == senderName).FirstOrDefault();
                if (userProfile != null)
                {
                    messages = messages.Where(s => s.SenderId == userProfile.Id && s.ReceiverId == User.Identity.GetUserId());
                }
                else
                {
                    messages = Array.Empty<Message>();
                }

            }
            switch (currentFilter)
            {
                case "read":
                    messages = messages.Where(m => m.IsViewed);
                    break;
                case "unread":
                    messages = messages.Where(m => !m.IsViewed);
                    break;

                default:
                    break;
            }
            switch (sortOrder)
            {
                case "name_desc":
                    //messages = messages.OrderByDescending(s => s.SenderName);
                    break;
                case "Date":
                    messages = messages.OrderBy(s => s.CreatedDate);
                    break;
                case "date_desc":
                    messages = messages.OrderByDescending(s => s.CreatedDate);
                    break;

                default:  // Name ascending 
                    messages = messages.OrderByDescending(s => s.CreatedDate);
                    break;
            }


            //var groups1 = messages.GroupBy(m => m.SenderId, (k,list) => list.LastOrDefault());
            //var groups2 = messages.GroupBy(m => m.SenderId, (k, list) => list.LastOrDefault());
            var group = from message in messages
                        group message by new { message.SenderId, message.ReceiverId };
            //var group = from message in messages
            //            group message by new { message.SenderId, message.ReceiverId }  into m 
            MessageListViewModel model = new MessageListViewModel()
            {
                Messages = new List<MessageViewModel>()
            };
            foreach (var message in messages)
            {
                model.Messages.Add(new MessageViewModel()
                {
                    Id = message.Id,
                    CreatedDate = message.CreatedDate,
                    IsViewed = message.IsViewed,
                    MessageBody = message.MessageBody,
                    ReceiverId = message.ReceiverId,
                    SenderId = message.SenderId,
                    SenderName = _db.Users.Get(message.SenderId).UserName,
                    Subject = message.Subject
                });
            }


            return View(model);
        }

        public ActionResult Sent(string sortOrder, string searchString, string receiverName, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            //ViewBag.FilterParam = String.IsNullOrEmpty(filter) ? "all" : "";
            ViewData["ReceiverName"] = receiverName;
            ViewData["SearchString"] = searchString;
            var messages = _db.Messages.Find(m => m.SenderId == User.Identity.GetUserId() && !m.SenderDeleted);

            //var messages = _db.Messages.Find(m => m.ReceiverId == User.Identity.GetUserId());

            if (!String.IsNullOrEmpty(searchString))
            {
                messages = messages.Where(s => s.MessageBody.Contains(searchString) && s.SenderId == User.Identity.GetUserId());
            }
            if (!String.IsNullOrEmpty(receiverName))
            {
                var userProfile = _db.UserProfiles.Find(m => m.ApplicationUser.UserName == receiverName).FirstOrDefault();
                if (userProfile != null)
                {
                    messages = messages.Where(s => s.ReceiverId == userProfile.Id && s.SenderId == User.Identity.GetUserId());
                }
                else
                {
                    messages = Array.Empty<Message>();
                }

            }
            switch (sortOrder)
            {
                case "name_desc":
                    //messages = messages.OrderByDescending(s => s.SenderName);
                    break;
                case "Date":
                    messages = messages.OrderBy(s => s.CreatedDate);
                    break;
                case "date_desc":
                    messages = messages.OrderByDescending(s => s.CreatedDate);
                    break;

                default:  // Name ascending 
                    messages = messages.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            MessageListViewModel model = new MessageListViewModel()
            {
                Messages = new List<MessageViewModel>()
            };
            foreach (var message in messages)
            {
                model.Messages.Add(new MessageViewModel()
                {
                    Id = message.Id,
                    CreatedDate = message.CreatedDate,
                    IsViewed = message.IsViewed,
                    MessageBody = message.MessageBody,
                    ReceiverId = message.ReceiverId,
                    SenderId = message.SenderId,
                    ReceiverName = _db.Users.Get(message.ReceiverId).UserName,
                    Subject = message.Subject
                });
            }
            return View(model);
        }


        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var user = _db.UserProfiles.Get(User.Identity.GetUserId());
                if (user != null)
                {

                    var message = _db.Messages.Get(id.ToString());
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
                        _db.Save();
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
                var user = _db.UserProfiles.Get(User.Identity.GetUserId());
                if (user != null)
                {

                    var message = _db.Messages.Get(id.ToString());
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
                        _db.Save();
                        return Json(new { Success = true });
                    }



                }
            }
            return Json(new { Success = false });

        }

        public JsonResult SetViewedAjax(int? id)
        {
            if (id != null)
            {
                var user = _db.UserProfiles.Get(User.Identity.GetUserId());
                if (user != null)
                {

                    var message = _db.Messages.Get(id.ToString());
                    if (message.ReceiverId == User.Identity.GetUserId() || message.SenderId == User.Identity.GetUserId())
                    {
                        message.IsViewed = true;
                        _db.Save();
                        return Json(new { Success = true });
                    }



                }
            }
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
        [ValidateAntiForgeryToken]
        public ActionResult New(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Поменять(Не проверять на наличие оффера)                
                var user = _db.UserProfiles.GetAll().FirstOrDefault(m => m.Id == model.ReceiverId);

                if (user != null)
                {
                    var message = new Message
                    {
                        IsViewed = false,
                        ReceiverId = model.ReceiverId,
                        SenderId = User.Identity.GetUserId(),
                        MessageBody = model.MessageBody,
                        CreatedDate = DateTime.Now,
                        Subject = model.Subject
                    };
                    _messageService.SendMessage(message);
                    return Redirect(Request.UrlReferrer.ToString());
                }
                return HttpNotFound("User does not exist!");
            }

            return View(model);
        }

        // GET: Message/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Message message = db.Messages.Find(id);
        //    if (message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.UserProfileId = new SelectList(db.UserProfiles, "Id", "Discription", message.UserProfileId);
        //    return View(message);
        //}

        // POST: Message/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Subject,MessageBody,ParentMessageId,IsViewed,SenderId,CreatedDate,ViewedDate,UserProfileId")] Message message)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(message).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.UserProfileId = new SelectList(db.UserProfiles, "Id", "Discription", message.UserProfileId);
        //    return View(message);
        //}

        // GET: Message/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Message message = db.Messages.Find(id);
        //    if (message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(message);
        //}

        // POST: Message/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Message message = db.Messages.Find(id);
        //    db.Messages.Remove(message);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}