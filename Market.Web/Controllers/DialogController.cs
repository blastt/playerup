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
    [Authorize]
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
            string dialogWithUserImage = null;
            if (dialogId != null)
            {
                Dialog dialog = _dialogService.GetDialog(dialogId.Value);
                if (dialog != null && (_dialogService.GetUserDialogs(currentUserId).Count() != 0))
                {

                    if (dialog.CompanionId == currentUserId)
                    {
                        dialogWithUserId = dialog.CreatorId;
                        dialogWithUserImage = dialog.Creator.ImagePath;
                    }
                    else if (dialog.CreatorId == currentUserId)
                    {
                        dialogWithUserId = dialog.CompanionId;
                        dialogWithUserImage = dialog.Companion.ImagePath;
                    }                                                             
                    
                    if (dialogWithUserId == null)
                    {
                        return HttpNotFound();
                    }
                    foreach (var message in dialog.Messages.Where(m => m.SenderId != currentUserId))
                    {
                        message.ToViewed = true;
                    }
                    _messageService.SaveMessage();
                    var model = Mapper.Map<Dialog, DialogViewModel>(dialog);
                    model.otherUserId = dialogWithUserId;
                    model.otherUserImage = dialogWithUserImage;
                    
                    return View(model);
                }
            }
            return HttpNotFound();
        }
    }
}