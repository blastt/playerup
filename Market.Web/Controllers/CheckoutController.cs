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
        private readonly IOrderService _orderService;
        private readonly IOfferService _offerService;
        private readonly IAccountInfoService _accountInfoService;
        public CheckoutController(IUserProfileService userProfileService, IOrderService orderService, IOfferService offerService, IAccountInfoService accountInfoService)
        {
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
            if (offer != null && offer.Order == null)
            {

                offer.Order = new Order
                {
                    BuyerChecked = false,
                    SellerChecked = false,
                    OrderStatus = Status.OrderCreated,
                    BuyerId = User.Identity.GetUserId(),
                    SellerId = model.SellerId,
                    DateCreated = DateTime.Now
                };
                _offerService.SaveOffer();

                var userProfiles = _userProfileService.GetUserProfiles()
                    .Where(u => u.ApplicationUser.Roles.Contains(new IdentityUserRole() { RoleId = "2", UserId = u.Id }));
                foreach (var user in userProfiles)
                {
                    user.Messages.Add(new Message()
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
        public ActionResult ProvideData(string moderatorId)
        {
            AccountInfoViewModel model = new AccountInfoViewModel
            {
                ModeratorId = moderatorId
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ProvideData(AccountInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accountInfo = AutoMapper.Mapper.Map<AccountInfoViewModel, AccountInfo>(model);
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
                    var order = _orderService.GetOrders().Where(o => o.Offer.SteamLogin == model.SteamLogin && o.ModeratorId == moderator.Id).FirstOrDefault();
                    if (order != null && order.AccountInfo == null && order.OrderStatus == Status.SellerProviding)
                    {
                        order.BuyerChecked = false;
                        order.SellerChecked = false;
                        order.OrderStatus = Status.AdminChecking;
                        accountInfo.ModeratorId = moderator.Id;
                        order.AccountInfo = accountInfo;
                        _orderService.SaveOrder();
                        return RedirectToAction("ProvideData", new { moderatorId = moderator.Id });
                    }
                }
            }

            return HttpNotFound("fefwefww");
        }


        public ActionResult ConfirmOrder(int? orderId)
        {
            if (orderId != null)
            {
                var order = _orderService.GetOrder(orderId.Value);
                if (order != null && order.BuyerId == User.Identity.GetUserId() && order.OrderStatus == Status.BuyerConfirming)
                {
                    order.BuyerChecked = false;
                    order.SellerChecked = false;
                    order.OrderStatus = Status.PayingToSeller;
                    _orderService.SaveOrder();
                    return View();
                }
            }
            return HttpNotFound("qqqq");
        }
    }
}