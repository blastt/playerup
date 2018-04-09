using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    public class DialogController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMessageService _messageService;
        private readonly IOfferService _offerService;
        private readonly IDialogService _dialogService;
        public DialogController(IMessageService messageService, IOfferService offerService, IUserProfileService userProfileService, IDialogService dialogService)
        {
            _messageService = messageService;
            _offerService = offerService;
            _userProfileService = userProfileService;
            _dialogService = dialogService;
        }

        public ActionResult Details(int? dialogId)
        {
            string currentUserId = User.Identity.GetUserId();
            string dialogWithUserId = null;
            if (dialogId != null)
            {
                Dialog dialog = _dialogService.GetDialog(dialogId.Value);
                if (dialog != null && (dialog.Users.Any(u => u.Id == currentUserId)))
                {

                    dialogWithUserId = dialog.Users.FirstOrDefault(u => u.Id != currentUserId).Id;
                    
                       
                    
                    if (dialogWithUserId == null)
                    {
                        return HttpNotFound("Error");
                    }
                    foreach (var message in dialog.Messages.Where(m => m.SenderId != currentUserId))
                    {
                        message.ToViewed = true;
                    }
                    _messageService.SaveMessage();
                    var model = Mapper.Map<Dialog, DialogViewModel>(dialog);
                    model.otherUserId = dialogWithUserId;
                    
                    return View(model);
                }
            }
            return HttpNotFound("Error");
        }
    }
}