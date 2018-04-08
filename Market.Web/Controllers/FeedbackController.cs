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

                Order order = null;//.Where(o => o.Id == model.OrderId && o.Buyer.Id == model.ReceiverId && o.Seller.Id == User.Identity.GetUserId() && !o.BuyerFeedbacked &&
            //(o.OrderStatus == Status.PayingToSeller || o.OrderStatus == Status.ClosedSeccessfuly)).FirstOrDefault();
                foreach (var o in _orderService.GetOrders())
                {
                    if (o.Id == model.OrderId && o.Buyer.Id == model.ReceiverId && o.Seller.Id == User.Identity.GetUserId() && !o.BuyerFeedbacked)
                    {
                        var orderStatus = o.OrderStatuses.LastOrDefault();
                        if(orderStatus != null)
                        {
                            if(orderStatus.Value == "payingToSeller" || orderStatus.Value == "closedSeccessfuly")
                            {
                                order = o;
                            }
                        }
                    }
                }
                if (order != null)
                {
                    order.BuyerFeedbacked = true;
                    var feedback = new Feedback
                    {
                        Comment = model.Comment,
                        DateLeft = DateTime.Now,
                        Grade = model.Grade,
                        SenderId = User.Identity.GetUserId(),
                        ReceiverId = model.ReceiverId,
                        OfferHeader = order.Offer.Header,
                        OfferId = order.Offer.Id.ToString()
                    };
                    order.Buyer.Feedbacks.Add(feedback);
                    _feedbackService.SaveFeedback();
                    return View(model);
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
            //if (model.ReceiverId != null && ModelState.IsValid && Enum.IsDefined(typeof(Emotions), model.Grade))
            //{

            //    var order = _orderService.GetOrders().Where(o => o.Id == model.OrderId && o.Seller.Id == model.ReceiverId && o.BuyerId == User.Identity.GetUserId() && !o.SellerFeedbacked &&
            //(o.OrderStatus == Status.PayingToSeller || o.OrderStatus == Status.ClosedSeccessfuly)).FirstOrDefault();
            //    if (order != null)
            //    {
            //        order.SellerFeedbacked = true;
            //        var feedback = new Feedback
            //        {
            //            Comment = model.Comment,
            //            DateLeft = DateTime.Now,
            //            Grade = model.Grade,
            //            SenderId = User.Identity.GetUserId(),
            //            ReceiverId = model.ReceiverId,
            //            OfferHeader = order.Offer.Header,
            //            OfferId = order.Offer.Id.ToString()
            //        };
            //        order.Seller.Feedbacks.Add(feedback);
            //        _feedbackService.SaveFeedback();
            //        return View(model);
            //    }
            //}

            //return HttpNotFound("Ошибка");

            if (model.ReceiverId != null && ModelState.IsValid && Enum.IsDefined(typeof(Emotions), model.Grade))
            {

                Order order = null;//.Where(o => o.Id == model.OrderId && o.Buyer.Id == model.ReceiverId && o.Seller.Id == User.Identity.GetUserId() && !o.BuyerFeedbacked &&
                                   //(o.OrderStatus == Status.PayingToSeller || o.OrderStatus == Status.ClosedSeccessfuly)).FirstOrDefault();
                foreach (var o in _orderService.GetOrders())
                {
                    if (o.Id == model.OrderId && o.Seller.Id == model.ReceiverId && o.BuyerId == User.Identity.GetUserId() && !o.SellerFeedbacked)
                    {
                        var orderStatus = o.OrderStatuses.LastOrDefault();
                        if (orderStatus != null)
                        {
                            if (orderStatus.Value == "payingToSeller" || orderStatus.Value == "closedSeccessfuly")
                            {
                                order = o;
                            }
                        }
                    }
                }
                if (order != null)
                {
                    order.BuyerFeedbacked = true;
                    var feedback = new Feedback
                    {
                        Comment = model.Comment,
                        DateLeft = DateTime.Now,
                        Grade = model.Grade,
                        SenderId = User.Identity.GetUserId(),
                        ReceiverId = model.ReceiverId,
                        OfferHeader = order.Offer.Header,
                        OfferId = order.Offer.Id.ToString()
                    };
                    order.Seller.Feedbacks.Add(feedback);
                    _feedbackService.SaveFeedback();
                    return View(model);
                }
            }

            return HttpNotFound();
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