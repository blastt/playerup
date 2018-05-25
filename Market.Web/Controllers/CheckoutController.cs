using AutoMapper;
using Hangfire;
using Market.Model.Models;
using Market.Service;
using Market.Web.Hangfire;
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
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IOrderStatusService _orderStatusService;
        private readonly IBillingService _billingService;
        private readonly ITransactionService _transactionService;
        private readonly IOrderService _orderService;
        private readonly IOfferService _offerService;
        private readonly IAccountInfoService _accountInfoService;
        public CheckoutController(IUserProfileService userProfileService, IOrderService orderService,
            IOfferService offerService, IAccountInfoService accountInfoService,
            IOrderStatusService orderStatusService, IBillingService billingService,
            ITransactionService transactionService)
        {
            _orderStatusService = orderStatusService;
            _userProfileService = userProfileService;
            _orderService = orderService;
            _offerService = offerService;
            _accountInfoService = accountInfoService;
            _billingService = billingService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public ActionResult Checkoutme(int? offerId)
        {
            if (offerId != null)
            {
                var offer = _offerService.GetOffer(offerId.Value);
                if (offer != null && offer.Order == null && offer.State == OfferState.active && offer.UserProfileId != User.Identity.GetUserId())
                {
                    CheckoutViewModel model = new CheckoutViewModel()
                    {
                        OfferHeader = offer.Header,
                        OfferId = offer.Id,
                        Game = offer.Game.Name,
                        SellerPaysMiddleman = offer.SellerPaysMiddleman,
                        MiddlemanPrice = offer.MiddlemanPrice.Value,
                        OrderSum = offer.Price,
                        Quantity = 1,
                        SellerId = offer.UserProfile.Id
                    };
                    if (offer.SellerPaysMiddleman)
                    {
                        model.OrderSum = offer.Price;
                    }
                    else
                    {
                        model.OrderSum = offer.Price + offer.MiddlemanPrice.Value;
                    }
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
            if (offer != null && offer.Order == null && offer.State == OfferState.active && offer.UserProfileId != User.Identity.GetUserId())
            {
                offer.Order = new Order
                {
                    BuyerId = User.Identity.GetUserId(),
                    SellerId = offer.UserProfileId,
                    DateCreated = DateTime.Now
                };
                offer.State = OfferState.closed;

                offer.Order.StatusLogs.AddLast(new StatusLog()
                {
                    OldStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.OrderCreating),
                    NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerPaying),
                    TimeStamp = DateTime.Now
                });
                offer.Order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerPaying);

                //_offerService.UpdateOffer(offer);
                

                var userProfiles = _userProfileService.GetUserProfiles()
                    .Where(u => u.ApplicationUser.Roles.Contains(new IdentityUserRole() { RoleId = "1", UserId = u.Id }));
                foreach (var user in userProfiles)
                {
                    user.MessagesAsReceiver.Add(new Message()
                    {
                        CreatedDate = DateTime.Now,
                        MessageBody = "Get"
                    });
                }
                TempData["orderBuyStatus"] = "Заказ создан!";

                if (offer.JobId != null)
                {
                    BackgroundJob.Delete(offer.JobId);
                    offer.JobId = null;
                }
                _offerService.SaveOffer();

                offer.Order.JobId = MarketHangfire.SetOrderCloseJob(offer.Order.Id, TimeSpan.FromDays(1));

                _orderService.SaveOrder();

                return RedirectToAction("BuyDetails", "Order", new { id = offer.Order.Id });

            }
            return HttpNotFound();

        }

        public void Test()
        {

        }
        [HttpGet]
        public ActionResult Paid()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Paid(int? id)
        {
            // Добавить логику оплаты
            if (id != null)
            {
                Order order = _orderService.GetOrder(id.Value);
                if (order != null && order.CurrentStatus.Value == OrderStatuses.BuyerPaying)
                {
                    var mainCup = _userProfileService.GetUserProfileByName("palyerup");
                    if (mainCup != null)
                    {
                        var seller = order.Seller;
                        var buyer = order.Buyer;

                        if (order.Offer.SellerPaysMiddleman)
                        {
                            order.Sum = order.Offer.Price;
                            order.AmmountSellerGet = order.Offer.Price - order.Offer.MiddlemanPrice.Value;
                            if (buyer.Balance >= order.Sum)
                            {
                                _transactionService.CreateTransaction(new Transaction
                                {
                                    Amount = order.Sum,
                                    Order = order,
                                    Receiver = mainCup,
                                    Sender = buyer,
                                    TransactionDate = DateTime.Now
                                });
                                buyer.Balance -= order.Sum;
                                mainCup.Balance += order.Sum;
                            }
                            else
                            {
                                return View("NotEnoughMoney");
                            }
                        }
                        else
                        {
                            order.Sum = order.Offer.Price + order.Offer.MiddlemanPrice.Value;
                            order.AmmountSellerGet = order.Offer.Price;
                            if (buyer.Balance >= order.Offer.Order.Sum)
                            {
                                _transactionService.CreateTransaction(new Transaction
                                {
                                    Amount = order.Sum,
                                    Order = order,
                                    Receiver = mainCup,
                                    Sender = buyer,
                                    TransactionDate = DateTime.Now
                                });
                                buyer.Balance -= order.Sum;
                                mainCup.Balance += order.Sum;
                            }
                            else
                            {
                                return View("NotEnoughMoney");
                            }
                        }
                        order.StatusLogs.AddLast(new StatusLog()
                        {
                            OldStatus = order.CurrentStatus,
                            NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding),
                            TimeStamp = DateTime.Now
                        });
                        order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding);

                        order.BuyerChecked = false;
                        order.SellerChecked = false;

                        if (order != null)
                        {
                            BackgroundJob.Delete(order.JobId);
                            order.JobId = null;
                        }

                        _orderService.SaveOrder();
                        
                        order.JobId = MarketHangfire.SetOrderCloseJob(order.Id, TimeSpan.FromDays(1));
                        //_orderService.UpdateOrder(order);
                        _orderService.SaveOrder();
                        TempData["orderBuyStatus"] = "Оплата прошла успешно";
                        return RedirectToAction("BuyDetails", "Order", new { id = order.Id });
                    }

                }
            }
            return View();

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
                    order.StatusLogs.AddLast(new StatusLog()
                    {
                        OldStatus = order.CurrentStatus,
                        NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.ClosedSuccessfully),
                        TimeStamp = DateTime.Now
                    });
                    order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.ClosedSuccessfully);
                    order.BuyerChecked = false;
                    order.SellerChecked = false;
                    //_orderService.UpdateOrder(order);
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
                        if (role.RoleId == "2" && role.UserId == moderator.Id)
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
                                order.StatusLogs.AddLast(new StatusLog()
                                {
                                    OldStatus = order.CurrentStatus,
                                    NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MidddlemanChecking),
                                    TimeStamp = DateTime.Now
                                });
                                order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MidddlemanChecking);


                                order.BuyerChecked = false;
                                order.SellerChecked = false;
                                order.AccountInfo = accountInfo;
                                if (order.JobId != null)
                                {
                                    BackgroundJob.Delete(order.JobId);
                                    order.JobId = null;
                                }
                                //order.JobId = MarketHangfire.SetOrderCloseJob(order.Id, TimeSpan.FromMinutes(5));
                                _orderService.SaveOrder();
                                TempData["orderSellStatus"] = "Ваши данные были отправлены на проверку гаранту";
                                return RedirectToAction("SellDetails", "Order", new { id = order.Id });
                            }
                        }
                    }
                }

            }
            return HttpNotFound();
        }

        public ActionResult ConfirmOrder(int? id)
        {
            if (id != null)
            {
                bool result = _orderService.ConfirmOrder(id.Value, User.Identity.GetUserId());
                var order = _orderService.GetOrder(id.Value);
                if (result && order != null)
                {
                    if (order != null)
                    {
                        BackgroundJob.Delete(order.JobId);
                        order.JobId = null;
                    }
                    
                    _orderService.SaveOrder();

                    order.JobId = MarketHangfire.SetLeaveFeedbackJob(order.SellerId, order.BuyerId, order.Id, TimeSpan.FromDays(15));

                    _orderService.SaveOrder();
                    TempData["orderBuyStatus"] = "Спасибо за подтверждение сделки! Сделка успешно закрыта.";
                    return RedirectToAction("BuyDetails", "Order", new { id });
                }
            }
            return HttpNotFound();
        }
    }
}