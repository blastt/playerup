using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TinifyAPI;

namespace Trader.WEB.Controllers
{
    public class ProfileController : Controller
    {

        private readonly IUserProfileService _userProfileService;
        private readonly IFeedbackService _feedbackService;
        private readonly IOfferService _offerService;
        private readonly IGameService _gameService;
        private readonly IFilterService _filterService;
        private readonly IFilterItemService _filterItemService;
        public int pageSize = 7;
        public ProfileController(IOfferService offerService, IFeedbackService feedbackService, IGameService gameService, IFilterService filterService, IFilterItemService filterItemService, IUserProfileService userProfileService)
        {
            _offerService = offerService;
            _gameService = gameService;
            _filterService = filterService;
            _filterItemService = filterItemService;
            _userProfileService = userProfileService;
            _feedbackService = feedbackService;
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

            if (profile.FeedbacksMy != null)
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
        public ActionResult Info(string userName)
        {
            var profile = _userProfileService.GetUserProfiles(u => u.Name == userName, u => u.Offers, u => u.FeedbacksMy, u => u.OrdersAsSeller, u => u.OrdersAsSeller.Select(m => m.StatusLogs), u => u.OrdersAsSeller.Select(m => m.StatusLogs.Select(f => f.NewStatus))).SingleOrDefault();
            if (profile == null)
            {
                return HttpNotFound();
            }
            var model = Mapper.Map<UserProfile, InfoUserProfileViewModel>(profile);
            model.CurrentUserId = User.Identity.GetUserId();
            model.InfoUserId = profile.Id;
            model.OffersViewModel.Offers = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(profile.Offers.Where(o => o.State == OfferState.active));
            model.FeedbacksViewModel.Feedbacks = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(profile.FeedbacksMy);
            model.SuccessOrderRate = profile.OrdersAsSeller.Where(o => o.StatusLogs.Any(s => s.NewStatus.Value == OrderStatuses.ClosedSuccessfully)).Count();
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

        public string Balance()
        {
            string userId = User.Identity.GetUserId();
            decimal balance = 0;
            if (userId != null)
            {
                UserProfile profile = _userProfileService.GetUserProfileById(userId);
                if (profile != null)
                {
                    balance = profile.Balance;
                }
            }
            return balance.ToString("C");
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

        public string Photo(string userId)
        {
            // get EF Database
            UserProfile profile = _userProfileService.GetUserProfileById(userId);
            // find the user. I am skipping validations and other checks.
            if (profile != null)
            {


                return profile.Avatar32Path;

            }
            return null;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Upload()
        {
            var userProfile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            if (userProfile != null)
            {                
                var model = Mapper.Map<UserProfile, UserProfileViewModel>(userProfile);
                return View(model);               
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Upload(HttpPostedFileBase image)
        {
            var user = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            if (user != null)
            {
                if (image != null && image.ContentLength <= 1000000 && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                {
                    string extName = System.IO.Path.GetExtension(image.FileName);
                    string fileName32 = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);
                    string fileName48 = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);
                    string fileName96 = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);

                    // сохраняем файл в папку Files в проекте
                    string fullPath32 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\Avatars", fileName32);
                    string urlPath32 = Url.Content("~/Content/Images/Avatars/" + fileName32);
                    string fullPath48 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\Avatars", fileName48);
                    string urlPath48 = Url.Content("~/Content/Images/Avatars/" + fileName48);
                    string fullPath96 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\Avatars", fileName96);
                    string urlPath96 = Url.Content("~/Content/Images/Avatars/" + fileName96);

                    Tinify.Key = ConfigurationManager.AppSettings["TINYPNG_APIKEY"];
                    //Default.png
                    
                        var name32 = user.Avatar32Path.Split('/').LastOrDefault();
                        var name48 = user.Avatar48Path.Split('/').LastOrDefault();
                        var name96 = user.Avatar96Path.Split('/').LastOrDefault();
                        if (name32 != null && name48 != null && name96 != null)
                        {
                            if (name32 != "Default32.png" && name48 != "Default48.png" && name96 != "Default96.png")
                            {
                                System.IO.File.Delete(Server.MapPath(user.Avatar32Path));
                                System.IO.File.Delete(Server.MapPath(user.Avatar48Path));
                                System.IO.File.Delete(Server.MapPath(user.Avatar96Path));
                            }
                        }
                    
                        
                    image.SaveAs(fullPath32);
                    image.SaveAs(fullPath48);
                    image.SaveAs(fullPath96);
                    try
                    {
                        using (var s = Tinify.FromFile(fullPath32))
                        {
                            var resized = s.Resize(new
                            {
                                method = "fit",
                                width = 32,
                                height = 32
                            });
                            await resized.ToFile(fullPath32);
                        }
                        using (var s = Tinify.FromFile(fullPath48))
                        {
                            var resized = s.Resize(new
                            {
                                method = "fit",
                                width = 48,
                                height = 48
                            });
                            await resized.ToFile(fullPath48);
                        }
                        using (var s = Tinify.FromFile(fullPath96))
                        {
                            var resized = s.Resize(new
                            {
                                method = "fit",
                                width = 96,
                                height = 96
                            });
                            await resized.ToFile(fullPath96);
                        }
                    }
                    catch (System.Exception)
                    {
                        // ignored
                    }


                    user.Avatar32Path = urlPath32;
                        user.Avatar48Path = urlPath48;
                        user.Avatar96Path = urlPath96;
                        _userProfileService.SaveUserProfile();

                        //catch (TinifyAPI.Exception)
                        //{
                        //    return HttpNotFound();
                        //}

                        //catch (System.Exception)
                        //{
                        //    return HttpNotFound();
                        //}



                        return RedirectToAction("Buy", "Offer");
                    
                    
                    
                }
                
            }
            return HttpNotFound();
            
        }


    }
}