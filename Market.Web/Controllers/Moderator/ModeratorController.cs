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
        private readonly IOrderStatusService _orderStatusService;
        public ModeratorController(IOrderService orderService, IAccountInfoService accountInfoService, IUserProfileService userProfileService, IOrderStatusService orderStatusService)
        {
            _orderService = orderService;
            _orderStatusService = orderStatusService;
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
           
            var orders = new List<Order>();//.Where(o => o.OrderStatus == Status.OrderCreated);

            foreach (var order in _orderService.GetOrders())
            {
                var currentOrderStatus = order.OrderStatuses.OrderBy(o => o.DateFinished).LastOrDefault();

                if (currentOrderStatus.Value == "adminWating")
                {
                    orders.Add(order);
                }
            }
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
                var currentOrderStatus = order.OrderStatuses.OrderBy(o => o.DateFinished).LastOrDefault();
                if (currentOrderStatus != null)
                {
                    if (order != null && currentOrderStatus.Value == "adminWating")
                    {
                        OrderStatus orderStatus = new OrderStatus
                        {
                            Value = "sellerProviding",
                            Name = "Продавец предоставляет информацию гаранту",
                            FinisedName = "Гарант найден",
                            DateFinished = DateTime.Now
                        };
                        
                        order.BuyerChecked = false;
                        order.SellerChecked = false;
                        order.OrderStatuses.Add(orderStatus);
                        order.ModeratorId = User.Identity.GetUserId();
                        _orderService.SaveOrder();
                        return RedirectToAction("MyOrderList");
                        
                        
                    }
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
                        if(buyerOrder != null)
                        {
                            var currentOrderStatus = buyerOrder.OrderStatuses.OrderBy(o => o.DateFinished).LastOrDefault();
                            if (currentOrderStatus != null)
                            {
                                if (buyer != null && buyerOrder != null && currentOrderStatus.Value == "adminChecking")
                                {
                                    OrderStatus orderStatus = new OrderStatus
                                    {
                                        Value = "buyerConfirming",
                                        Name = "Покупатель подтверждает получение",
                                        FinisedName = "Гарант проверил данные",
                                        DateFinished = DateTime.Now
                                    };

                                    if (orderStatus != null)
                                    {
                                        buyerOrder.BuyerChecked = false;
                                        buyerOrder.SellerChecked = false;
                                        buyerOrder.OrderStatuses.Add(orderStatus);
                                        accInfo.BuyerId = buyer.Id;
                                        _accountInfoService.UpdateAccountInfo(accInfo);
                                        buyerOrder.AccountInfo = accInfo;
                                        _orderService.SaveOrder();
                                        return RedirectToAction("ProvideDataToBuyer", new { orderId = buyerOrder.Id });
                                    }
                                    
                                }
                            }
                            
                        }
                        
                    }
                    
                }
            }

            return HttpNotFound("fefwefww");
        }

    }
}