using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Market.Web.ViewModels.Checkout;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        // GET: Checkout
        // This method must be called after payment
        public ActionResult Pay(CheckoutViewModel model)
        {
            //UserProfile buyer = _db.UserProfiles.Get(User.Identity.GetUserId());
            Offer offer = _offerService.GetOffer(model.OfferId);
            if (offer != null && offer.Order == null && offer.State == OfferState.active)
            {
                offer.State = OfferState.closed;
                OrderStatus orderStatus = new OrderStatus
                {
                    Value = "adminWating",
                    FinisedName = "Заказ создан",
                    Name = "Поиск гаранта",
                    DateFinished = DateTime.Now
                };

                offer.Order = new Order
                {
                    BuyerChecked = false,
                    SellerChecked = false,
                    BuyerId = User.Identity.GetUserId(),
                    SellerId = model.SellerId,
                    DateCreated = DateTime.Now
                };

                offer.Order.OrderStatuses.Add(orderStatus);
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
                return RedirectToAction("BuyDetails", "Order");

            }
            return View();

        }

        // Provide data from seller to moderator
        [HttpGet]
        public ActionResult ProvideData(int? Id, string moderatorId, string buyerId, string sellerId)
        {
            if (Id != null && moderatorId != null && buyerId != null && sellerId != null)
            {
                Order order = _orderService.GetOrder(Id.Value);
                var currentOrderStatus = _orderStatusService.GetCurrentOrderStatus(order);
                if (sellerId == User.Identity.GetUserId())
                {
                    if (order.MiddlemanId == moderatorId && order.SellerId == sellerId &&
                    order.BuyerId == buyerId && currentOrderStatus.Value == "sellerProviding")
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
                        var currentOrderStatus = _orderStatusService.GetCurrentOrderStatus(order);
                        if (currentOrderStatus != null)
                        {
                            if (order.AccountInfo == null && currentOrderStatus.Value == "sellerProviding")
                            {
                                OrderStatus orderStatus = new OrderStatus
                                {
                                    Value = "adminChecking",
                                    Name = "Гарант проверяет данные",
                                    FinisedName = "Продавец предоставил данные гаранту",
                                    DateFinished = DateTime.Now
                                };                                
                                order.BuyerChecked = false;
                                order.SellerChecked = false;
                                order.OrderStatuses.Add(orderStatus);
                                accountInfo.ModeratorId = model.ModeratorId;
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
                    var currentOrderStatus = _orderStatusService.GetCurrentOrderStatus(order);
                    if (currentOrderStatus != null)
                    {
                        if (order.BuyerId == User.Identity.GetUserId() && currentOrderStatus.Value == "buyerConfirming")
                        {

                            OrderStatus orderStatus = new OrderStatus
                            {
                                Value = "payingToSeller",
                                Name = "Отправка средств продавцу",
                                FinisedName = "Покупатель подтвердил получение",
                                DateFinished = DateTime.Now
                            };

                            if (orderStatus != null)
                            {
                                order.BuyerChecked = false;
                                order.SellerChecked = false;
                                order.OrderStatuses.Add(orderStatus);
                                _orderService.SaveOrder();
                                return View();
                            }

                        }
                    }
                }

            }
            return HttpNotFound();
        }
    }
}