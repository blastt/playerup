using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Trader.WEB.Controllers
{
    public class ProfileController : Controller
    {

        private readonly IUserProfileService _userProfileService;
        private readonly IOfferService _offerService;
        private readonly IGameService _gameService;
        private readonly IFilterService _filterService;
        private readonly IFilterItemService _filterItemService;
        public int pageSize = 7;
        public ProfileController(IOfferService offerService, IGameService gameService, IFilterService filterService, IFilterItemService filterItemService, IUserProfileService userProfileService)
        {
            _offerService = offerService;
            _gameService = gameService;
            _filterService = filterService;
            _filterItemService = filterItemService;
            _userProfileService = userProfileService;
        }

        [Authorize]
        public ActionResult Offers()
        {
            var offers = _offerService.GetOffers();
            if (offers.Count() != 0)
            {
                offers = offers.Where(m => m.UserProfileId == User.Identity.GetUserId());
            }
            //var model = new OfferListViewModel
            //{
            //    Offers = offers                
            //};
            return View();
        }

        [Authorize]
        public ActionResult Feedbacks()
        {
            var profile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());

            if (profile.Feedbacks != null)
            {
                //FeedbackListViewModel model = new FeedbackListViewModel
                //{
                //    Feedbacks = profile.Feedbacks
                //};
                return View();
            }
            return View("Error");
            
        }

        //public ActionResult Messeges()
        //{
        //    var profile = _db.ClientProfiles.Find(m => m.NickName == prof).FirstOrDefault();

        //    if (profile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(profile);
        //}
        [Authorize]
        public ActionResult Settings()
        {            
            return View();
        }
        public ActionResult Info(string id)
        {
            var profile = _userProfileService.GetUserProfileById(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            var model = Mapper.Map<UserProfile, InfoUserProfileViewModel>(profile);
            double posProcent = 0;
            double negProcent = 0;
            int feedbackCount = model.NegativeFeedbacks + model.PositiveFeedbacks;
            if (model.PositiveFeedbacks != 0 || model.NegativeFeedbacks != 0)
            {
                if (model.PositiveFeedbacks == 0)
                {
                    model.NegativeFeedbacks = 100;
                }
                else
                {
                    negProcent = Math.Round((double)(100 * model.NegativeFeedbacks) / (feedbackCount), 2);
                }
                if (model.PositiveFeedbacks == 0)
                {
                    model.NegativeFeedbacks = 100;
                }
                else
                {
                    posProcent = Math.Round((double)(100 * model.PositiveFeedbacks) / (feedbackCount), 2);
                }
            }
            model.PositiveFeedbackProcent = posProcent;
            model.NegativeFeedbackProcent = negProcent;
            model.CurrentUserId = User.Identity.GetUserId();
            model.OffersViewModel.Offers = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(profile.Offers);
            model.FeedbacksViewModel.Feedbacks = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(profile.Feedbacks);
            var games = _gameService.GetGames();
            
            model.FeedbacksViewModel.PageInfo = new PageInfoViewModel
            {
                PageSize = 4,
                TotalItems = model.FeedbacksViewModel.Feedbacks.Count()
            };

            model.OffersViewModel.PageInfo = new PageInfoViewModel
            {
                PageSize = 4,
                TotalItems = model.OffersViewModel.Offers.Count()
            };

            return View(model);
        }


        //[HttpPost]
        //public new ActionResult Profile(ClientProfile profile)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(profile);
        //    }
        //    ClientProfile prof = _db.ClientProfiles.Get(profile.Id);
        //    prof.Discription = profile.Discription;
        //    _db.ClientProfiles.Update(prof);
        //    _db.Save();
        //    return View(prof);
        //}

        //[Authorize]
        //public ActionResult Comment(string id, string offerHeader)
        //{
        //    var fb = new FeedbackViewModel();
        //    UserProfile client = _db.UserProfiles.Find(m => m.Id == id).FirstOrDefault();
        //    string currentUserName = User.Identity.Name;
        //    if (client != null && currentUserName != null)
        //    {
        //        fb.UserProfileId = client.Id;
        //        //if (client.Feedbacks.Contains(new Feedback { WhoLeftName = currentUserName }, new NameEqualityComparer()))
        //        //    return Content("Вы уже осталяли комментарий этому пользователю");
        //    }
            
        //    return View(fb);
        //}

        //[HttpPost]
        //public ActionResult Comment(FeedbackViewModel fb)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(fb);
        //    }

        //    var feedback = new Feedback
        //    {
        //        SenderId = _db.UserProfiles.Get(User.Identity.GetUserId<string>()).Id,
        //        UserProfileId = fb.UserProfileId,
        //        Comment = fb.Comment,
        //        Grade = fb.Grade,
        //        Date = DateTime.Now
        //    };
        //    UserProfile profile = _db.UserProfiles.Get(feedback.UserProfileId);
        //    if (profile != null)
        //    {
        //        if (feedback.Grade == Emotions.Good)
        //        {
        //            profile.Positive++;
        //        }
        //        else if (feedback.Grade == Emotions.Bad)
        //        {
        //            profile.Negative++;
        //        }
        //        int rating = profile.Positive - profile.Negative;
        //        profile.Rating = rating;
        //        _db.Feedbacks.Create(feedback);
        //        _db.Save();
        //    }

        //    return RedirectToAction("List","Offer");
        //}
        //public string GetBalance(string userId)
        //{
        //    UserProfile profile = _userProfileService.GetUserProfileById(userId);
        //    if (profile != null)
        //    {
        //        return string.Format("{0} руб.", profile.Balance);
        //    }
        //    return "";
        //}

        public FileContentResult Photo(string userId)
        {
            // get EF Database
            UserProfile profile = _userProfileService.GetUserProfileById(userId);
            // find the user. I am skipping validations and other checks.
            if(profile != null)
            {
                
                    
                return File(profile.Avatar, "image/jpeg");
                                                             
            }
            return null;
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Upload(HttpPostedFileBase profile)
        {
            var user = _userProfileService.GetUserProfileById(User.Identity.GetUserId());

            // convert image stream to byte array
            byte[] image = new byte[profile.ContentLength];
            profile.InputStream.Read(image, 0, Convert.ToInt32(profile.ContentLength));

            user.Avatar = image;

            // save changes to database
            _userProfileService.SaveUserProfile();

            return RedirectToAction("List", "Offer");
        }


    }
}