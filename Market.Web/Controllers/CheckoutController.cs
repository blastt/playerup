using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Trader.WEB.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IOrderStatusService _orderStatusService;
        private readonly IOrderService _orderService;
        private readonly IOfferService _offerService;
        private readonly IAccountInfoService _accountInfoService;
        public CheckoutController(IUserProfileService userProfileService, IOrderService orderService, IOfferService offerService, IAccountInfoService accountInfoService, IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
            _userProfileService = userProfileService;
            _orderService = orderService;
            _offerService = offerService;
            _accountInfoService = accountInfoService;
        }

        [HttpGet]
        public ActionResult Checkoutme(int? offerId)
        {
            if (offerId != null)
            {
                var offer = _offerService.GetOffer(offerId.Value);
                if (offer != null && offer.Order == null)
                {
                    CheckoutViewModel model = new CheckoutViewModel()
                    {
                        OfferHeader = offer.Header,
                        OfferId = offer.Id,
                        OrderSum = offer.Price,
                        Game = offer.Game.Name,
                        Quantity = 1,
                        SellerId = offer.UserProfile.Id
                    };
                    return View(model);
                }
            }
            return HttpNotFound();
        }
        // GET: Checkout
        // This method must be called after payment
        [HttpPost]
        public ActionResult Checkoutme(CheckoutViewModel model)
        {
            //UserProfile buyer = _db.UserProfiles.Get(User.Identity.GetUserId());
            Offer offer = _offerService.GetOffer(model.OfferId);
            if (offer != null && offer.Order == null && offer.State == OfferState.active)
            {
                offer.Order = new Order
                {
                    BuyerId = User.Identity.GetUserId(),
                    SellerId = offer.UserProfileId,
                    Sum = 123321,
                    DateCreated = DateTime.Now
                };
                offer.State = OfferState.closed;

                offer.Order.StatusLogs.Add(new StatusLog()
                {
                    OldStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.OrderCreating),
                    NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerPaying),
                    TimeStamp = DateTime.Now
                });
                offer.Order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerPaying);

                _offerService.UpdateOffer(offer);
                _offerService.SaveOffer();

                var userProfiles = _userProfileService.GetUserProfiles()
                    .Where(u => u.ApplicationUser.Roles.Contains(new IdentityUserRole() { RoleId = "2", UserId = u.Id }));
                foreach (var user in userProfiles)
                {
                    user.MessagesAsReceiver.Add(new Message()
                    {
                        CreatedDate = DateTime.Now,
                        MessageBody = "Get"
                    });
                }
                return RedirectToAction("Paid", "Checkout", new { id = offer.Order.Id });

            }
            return View();

        }

        [HttpGet]
        public ActionResult Paid()
        {
            return View();
        }

        [HttpPost]
        public void Paid(int? orderId)
        {
            // Добавить логику оплаты
            if (orderId != null)
            {
                Order order = _orderService.GetOrder(orderId.Value);
                if (order != null && order.CurrentStatus.Value == OrderStatuses.BuyerPaying)
                {
                    order.StatusLogs.Add(new StatusLog()
                    {
                        OldStatus = order.CurrentStatus,
                        NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding),
                        TimeStamp = DateTime.Now
                    });
                    order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding);
                    order.BuyerChecked = false;
                    order.SellerChecked = false;
                    _orderService.UpdateOrder(order);
                    _orderService.SaveOrder();
                }
            }

        }

        // этот метод вывызается когда средства были доставлены продавцу
        [HttpPost]
        public void SellerWasPaid(int? orderId)
        {
            // Добавить логику оплаты
            if (orderId != null)
            {
                Order order = _orderService.GetOrder(orderId.Value);
                if (order != null && order.CurrentStatus.Value == OrderStatuses.PayingToSeller)
                {
                    order.StatusLogs.Add(new StatusLog()
                    {
                        OldStatus = order.CurrentStatus,
                        NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.ClosedSuccessfully),
                        TimeStamp = DateTime.Now
                    });
                    order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.ClosedSuccessfully);
                    order.BuyerChecked = false;
                    order.SellerChecked = false;
                    _orderService.UpdateOrder(order);
                    _orderService.SaveOrder();
                }
            }

        }

        private bool CheckSign(HttpContext context)
        {
            var secretKey = "sfewkf342o";
            var pars = new SortedDictionary<string, string>();
            var keys = context.Request.Form.AllKeys;
            foreach (var key in keys.Where(key => key.IndexOf("ik_") >= 0 && key != "ik_sign"))
                pars.Add(key, context.Request.Form[key]);
            var hash = string.Join(":", pars.Select(x => x.Value).ToArray().Concat(new[] { secretKey }));
            var md5 = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(hash))) == context.Request.Form["ik_sign"];
        }


        //public ActionResult Paid(int? Id, string moderatorId, string buyerId, string sellerId)
        //{
        //    if (Id != null && moderatorId != null && buyerId != null && sellerId != null)
        //    {
        //        Order order = _orderService.GetOrder(Id.Value);
        //        var currentOrderStatus = _orderStatusService.GetCurrentOrderStatus(order);
        //        if (sellerId == User.Identity.GetUserId())
        //        {
        //            if (order.MiddlemanId == moderatorId && order.SellerId == sellerId &&
        //            order.BuyerId == buyerId && currentOrderStatus.Value == "buyerPaying")
        //            {
        //                currentOrderStatus.DateFinished = DateTime.Now;
        //                OrderStatus orderStatus = new OrderStatus
        //                {
        //                    Value = "MiddlemanSearching",
        //                    Name = "Поиск гаранта",
        //                    FinisedName = "Гарант найден",
        //                };
        //                order.BuyerChecked = false;
        //                order.SellerChecked = false;
        //                order.OrderStatuses.AddLast(orderStatus);
        //                _orderService.UpdateOrder(order);
        //                _orderService.SaveOrder();
        //                return View();
        //            }
        //        }

        //    }
        //    return HttpNotFound();

        //}

        // Provide data from seller to moderator
        [HttpGet]
        public ActionResult ProvideData(int? Id, string moderatorId, string buyerId, string sellerId)
        {
            if (Id != null && moderatorId != null && buyerId != null && sellerId != null)
            {
                Order order = _orderService.GetOrder(Id.Value);
                if (sellerId == User.Identity.GetUserId())
                {
                    if (order.MiddlemanId == moderatorId && order.SellerId == sellerId &&
                    order.BuyerId == buyerId && order.CurrentStatus.Value == OrderStatuses.SellerProviding)
                    {
                        AccountInfoViewModel model = new AccountInfoViewModel
                        {
                            ModeratorId = moderatorId,
                            SteamLogin = order.Offer.AccountLogin,
                            BuyerId = buyerId,
                            SellerId = sellerId

                        };
                        return View(model);
                    }
                }

            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ProvideData(AccountInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accountInfo = Mapper.Map<AccountInfoViewModel, AccountInfo>(model);
                var moderator = _userProfileService.GetUserProfileById(model.ModeratorId);

                bool moderIsInrole = false;
                if (moderator != null)
                {
                    foreach (var role in moderator.ApplicationUser.Roles)
                    {
                        if (role.RoleId == "1" && role.UserId == moderator.Id)
                        {
                            moderIsInrole = true;
                        }
                    }
                }
                if (moderIsInrole)
                {
                    var order = _orderService.GetOrder(model.SteamLogin, model.ModeratorId, model.SellerId, model.BuyerId);
                    if (order != null)
                    {
                        if (order.CurrentStatus != null)
                        {
                            if (order.AccountInfo == null && order.CurrentStatus.Value == OrderStatuses.SellerProviding)
                            {
                                order.StatusLogs.Add(new StatusLog()
                                {
                                    OldStatus = order.CurrentStatus,
                                    NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MidddlemanChecking),
                                    TimeStamp = DateTime.Now
                                });
                                order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MidddlemanChecking);


                                order.BuyerChecked = false;
                                order.SellerChecked = false;
                                order.AccountInfo = accountInfo;
                                _orderService.SaveOrder();
                                return RedirectToAction("ProvideData", new { moderatorId = model.ModeratorId });
                            }
                        }
                    }
                }
            }
            return HttpNotFound();
        }

        public ActionResult ConfirmOrder(int? orderId)
        {
            if (orderId != null)
            {
                var order = _orderService.GetOrder(orderId.Value);
                if (order != null)
                {
                    if (order.CurrentStatus != null)
                    {
                        if (order.BuyerId == User.Identity.GetUserId() && order.CurrentStatus.Value == OrderStatuses.BuyerConfirming)
                        {
                            order.StatusLogs.Add(new StatusLog()
                            {
                                OldStatus = order.CurrentStatus,
                                NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.PayingToSeller),
                                TimeStamp = DateTime.Now
                            });
                            order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.PayingToSeller);
                            
                            order.BuyerChecked = false;
                            order.SellerChecked = false;
                            
                            _orderService.SaveOrder();
                            return View();
                            

                        }
                    }
                }

            }
            return HttpNotFound();
        }
    }
}