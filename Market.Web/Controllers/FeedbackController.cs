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
        private readonly IOrderService _orderService;
        private readonly IUserProfileService _userProfileService;
        private readonly IOrderStatusService _orderStatusService;
        private const int pageSize = 3;
        public FeedbackController(IOfferService offerService, IFeedbackService feedbackService, IUserProfileService userProfileService, IOrderService orderService, IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
            _offerService = offerService;
            _feedbackService = feedbackService;
            _userProfileService = userProfileService;
            _orderService = orderService;
        }
        public ActionResult All()
        {

            return View();
        }



        public PartialViewResult FeedbackList(string sellerId, int page = 1, string filter = "all")
        {
            FeedbackListViewModel model = new FeedbackListViewModel();
            IEnumerable<Feedback> feedbacks = _feedbackService.GetFeedbacks().Where(m => m.UserId == sellerId);
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
        public ActionResult GiveToBuyer(string receiverId, int? orderId)
        {
            if (receiverId != null && orderId != null)
            {
                GiveFeedbackViewModel model = new GiveFeedbackViewModel()
                {
                    ReceiverId = receiverId,
                    OrderId = orderId.Value
                };
                return View(model);                
            }

            return HttpNotFound();           

            //ViewBag.UserProfileId = new SelectList(db.UserProfiles, "Id", "Discription");            
        }

        // POST: Feedback/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GiveToBuyer(GiveFeedbackViewModel model)
        {
            if (model.ReceiverId != null && ModelState.IsValid && Enum.IsDefined(typeof(Emotions), model.Grade))
            {
                if (model.OrderId != null)
                {
                    Order order = _orderService.GetOrder(model.OrderId.Value);

                    if (order != null && order.BuyerId == model.ReceiverId && order.SellerId == User.Identity.GetUserId() && !order.BuyerFeedbacked)
                    {
                        order.SellerFeedbacked = true;
                        var feedback = Mapper.Map<GiveFeedbackViewModel, Feedback>(model);
                        order.Feedback = feedback;
                        order.Buyer.Feedbacks.Add(feedback);
                        _feedbackService.CreateFeedback(feedback);
                        _feedbackService.SaveFeedback();
                        return View(model);
                    }
                }
            }           

            return HttpNotFound();
        }

        // GET: Feedback/Create
        public ActionResult GiveToSeller(string receiverId, int? orderId)
        {
            if (receiverId != null && orderId != null)
            {
                GiveFeedbackViewModel model = new GiveFeedbackViewModel()
                {
                    ReceiverId = receiverId,
                    OrderId = orderId.Value
                };
                return View(model);
            }

            return HttpNotFound();

            //ViewBag.UserProfileId = new SelectList(db.UserProfiles, "Id", "Discription");            
        }

        // POST: Feedback/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GiveToSeller(GiveFeedbackViewModel model)
        {

            if (model.ReceiverId != null && ModelState.IsValid && Enum.IsDefined(typeof(Emotions), model.Grade))
            {
                if (model.OrderId != null)
                {
                    Order order = _orderService.GetOrder(model.OrderId.Value);

                    if (order != null && order.Seller.Id == model.ReceiverId && order.BuyerId == User.Identity.GetUserId() && !order.SellerFeedbacked)
                    {
                        order.BuyerFeedbacked = true;
                        var feedback = Mapper.Map<GiveFeedbackViewModel, Feedback>(model);

                        _feedbackService.CreateFeedback(feedback);
                        _feedbackService.SaveFeedback();
                        return View(model);
                    }
                }                
            }
            return HttpNotFound();
        }

        public PartialViewResult FeedbackListInfo(SearchFeedbacksInfoViewModel searchInfo)
        {
            var feedbacks = _feedbackService.GetFeedbacks().Where(m => m.UserId == searchInfo.UserId);
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