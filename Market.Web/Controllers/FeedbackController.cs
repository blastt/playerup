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
    public class FeedbackController : Controller
    {

        private readonly IFeedbackService _feedbackService;
        private readonly IOfferService _offerService;
        private readonly IUserProfileService _userProfileService;
        private const int pageSize = 3;
        public FeedbackController(IOfferService offerService, IFeedbackService feedbackService, IUserProfileService userProfileService)
        {
            _offerService = offerService;
            _feedbackService = feedbackService;
            _userProfileService = userProfileService;
        }
        public ActionResult All()
        {

            return View();
        }



        public PartialViewResult FeedbackList(string receiverId, int page = 1, string filter = "all")
        {
            FeedbackListViewModel model = new FeedbackListViewModel();
            IEnumerable<Feedback> feedbacks = _feedbackService.GetFeedbacks().Where(m => m.ReceiverId == receiverId);
            IList<FeedbackViewModel> feedbackViewModels = Mapper.Map<List<Feedback>, List<FeedbackViewModel>>(feedbacks.ToList());
            model.Feedbacks = feedbackViewModels.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            model.PageInfo = new PageInfoViewModel()
            {
                PageSize = pageSize,
                PageNumber = page,
                TotalItems = feedbacks.Count()
            };
            return PartialView("_FeedbackList", model);
        }




        // GET: Feedback/Create
        public ActionResult Give(string senderId, int? id)
        {
            if (senderId == null || id == null)
            {
                return HttpNotFound("pass error");
            }
            GiveFeedbackViewModel model = new GiveFeedbackViewModel()
            {
                ReceiverId = senderId
            };

            //ViewBag.UserProfileId = new SelectList(db.UserProfiles, "Id", "Discription");
            return View(model);
        }

        // POST: Feedback/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Give(GiveFeedbackViewModel model, int id = 1)
        {

            if (ModelState.IsValid && Enum.IsDefined(typeof(Emotions), model.Grade))
            {
                var receiverUserProfile = _userProfileService.GetUserProfileById(model.ReceiverId);


                if (receiverUserProfile != null)
                {
                    var offer = receiverUserProfile.Offers.FirstOrDefault(m => m.Id == id);
                    if (offer != null)
                    {
                        //if (offer.Order != null)
                        //{
                        //    if (offer.Order.IsFeedbacked == false && offer.Order.OrderStatus == Status.Successfully)
                        //    {
                        var currentUserProfile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
                        var feedback = new Feedback
                        {
                            Comment = model.Comment,
                            DateLeft = DateTime.Now,
                            Grade = model.Grade,
                            Sender = currentUserProfile,
                            Receiver = receiverUserProfile,
                            OfferHeader = offer.Header,
                            OfferId = offer.Id.ToString()
                        };
                        //offer.Order.IsFeedbacked = true;
                        _feedbackService.CreateFeedback(feedback);
                        _feedbackService.SaveFeedback();
                        return RedirectToAction("Index");
                        //    }
                        //}
                    }
                }
                return HttpNotFound("User does not exist");

            }
            //ViewBag.UserProfileId = new SelectList(_db.UserProfiles, "Id", "Discription", feedback.UserProfileId);
            return View(model);
        }

        public PartialViewResult FeedbackListInfo(SearchFeedbacksInfoViewModel searchInfo)
        {
            var feedbacks = _feedbackService.GetFeedbacks().Where(m => m.ReceiverId == searchInfo.UserId);
            var modelFeedbacks = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(feedbacks);
            FeedbackListViewModel model = new FeedbackListViewModel
            {
                Feedbacks = modelFeedbacks,
                PageInfo = new PageInfoViewModel
                {
                    PageSize = 4,
                    PageNumber = searchInfo.Page,
                    TotalItems = modelFeedbacks.Count()
                },
                SearchInfo = new SearchViewModel
                {
                    Page = searchInfo.Page
                }
            };
            return PartialView("_FeedbackListInfo", model);
        }
        // GET: Feedback/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Feedback feedback = _db.Feedbacks.Find(id);
        //    if (feedback == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.UserProfileId = new SelectList(_db.UserProfiles, "Id", "Discription", feedback.UserProfileId);
        //    return View(feedback);
        //}

        //// POST: Feedback/Edit/5
        //// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        //// сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Grade,Comment,Date,WhoLeftId,WhoLeftName,OfferId,OfferHeader,UserProfileId")] Feedback feedback)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Entry(feedback).State = EntityState.Modified;
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.UserProfileId = new SelectList(db.UserProfiles, "Id", "Discription", feedback.UserProfileId);
        //    return View(feedback);
        //}

        //// POST: Feedback/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Feedback feedback = _db.Feedbacks.Find(id);
        //    _db.Feedbacks.Remove(feedback);
        //    _db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

    }
}