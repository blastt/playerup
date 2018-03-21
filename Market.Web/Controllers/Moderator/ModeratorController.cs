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

namespace Market.Web.Controllers.Moderator
{
    [Authorize(Roles = "Moderator")]
    public class ModeratorController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly IUserProfileService _userProfileService;

        public ModeratorController(IOrderService orderService, IAccountInfoService accountInfoService, IUserProfileService userProfileService)
        {
            _orderService = orderService;
            _accountInfoService = accountInfoService;
            _userProfileService = userProfileService;
        }
        // GET: Moderator
        public ActionResult Panel()
        {
            return View();
        }

        public ActionResult OrderList()
        {
            var orders = _orderService.GetOrders().Where(o => o.OrderStatus == Status.OrderCreated);
            var ordersViewModel = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders);
            OrderListViewModel model = new OrderListViewModel
            {
                Orders = ordersViewModel
            };
            return View(model);
        }

        public ActionResult MyOrderList()
        {
            var orders = _orderService.GetOrders().Where(o => o.ModeratorId == User.Identity.GetUserId());
            var ordersViewModel = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders);
            OrderListViewModel model = new OrderListViewModel
            {
                Orders = ordersViewModel
            };
            return View(model);
        }

        
        public ActionResult ProcessOrder(int? orderId)
        {
            if(orderId != null)
            {
                var order = _orderService.GetOrder(orderId.Value);
                if(order != null && order.OrderStatus == Model.Models.Status.OrderCreated)
                {
                    order.BuyerChecked = false;
                    order.SellerChecked = false;
                    order.OrderStatus = Model.Models.Status.SellerProviding;
                    order.ModeratorId = User.Identity.GetUserId();
                    _orderService.SaveOrder();
                    return RedirectToAction("MyOrderList");
                }
               
            }
            
            return HttpNotFound("fewfe");
        }

        // Provide data from moderator to buyer
        [HttpGet]
        public ActionResult ProvideDataToBuyer(int? orderId)
        {
            if (orderId != null)
            {
                var order = _orderService.GetOrder(orderId.Value);
                if (order != null)
                {
                    var accInfo = order.AccountInfo;
                    var model = Mapper.Map<AccountInfo, AccountInfoViewModel>(accInfo);
                    model.BuyerId = order.BuyerId;
                    model.ModeratorId = order.ModeratorId;
                    return View(model);
                }
            }
            return View();
        }

        // Provide data from moderator to buyer
        [HttpPost]
        public ActionResult ProvideDataToBuyer(AccountInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accInfo = _accountInfoService.GetAccountInfo(model.Id);
                if (accInfo != null)
                {
                    accInfo.Login = model.SteamLogin;
                    accInfo.Password = model.SteamPassword;
                    accInfo.Email = model.SteamEmail;
                    accInfo.AdditionalInformation = model.AdditionalInformation;

                    var buyer = _userProfileService.GetUserProfileById(model.BuyerId);
                    if(buyer != null)
                    {
                        var buyerOrder = buyer.Orders.Where(m => m.BuyerId == model.BuyerId && m.ModeratorId == User.Identity.GetUserId() && m.Offer.SteamLogin == model.SteamLogin).FirstOrDefault();
                        if (buyer != null && buyerOrder != null && buyerOrder.OrderStatus == Status.AdminChecking)
                        {
                            buyerOrder.BuyerChecked = false;
                            buyerOrder.SellerChecked = false;
                            buyerOrder.OrderStatus = Status.BuyerConfirming;
                            accInfo.BuyerId = buyer.Id;
                            _accountInfoService.UpdateAccountInfo(accInfo);
                            buyerOrder.AccountInfo = accInfo;
                            _orderService.SaveOrder();
                            return RedirectToAction("ProvideDataToBuyer", new { orderId = buyerOrder.Id });
                        }
                    }
                    
                }
            }

            return HttpNotFound("fefwefww");
        }

    }
}