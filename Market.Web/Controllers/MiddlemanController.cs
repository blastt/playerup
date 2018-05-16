using AutoMapper;
using Hangfire;
using Market.Model.Models;
using Market.Service;
using Market.Web.Hangfire;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    [Authorize(Roles = "Middleman")]
    public class MiddlemanController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly IUserProfileService _userProfileService;
        private readonly IOrderStatusService _orderStatusService;
        public MiddlemanController(IOrderService orderService, IAccountInfoService accountInfoService, IUserProfileService userProfileService, IOrderStatusService orderStatusService)
        {
            _orderService = orderService;
            _orderStatusService = orderStatusService;
            _accountInfoService = accountInfoService;
            _userProfileService = userProfileService;
        }
        // GET: Middleman
        public ActionResult OrderList()
        {
            var orders = new List<Order>();//.Where(o => o.OrderStatus == Status.OrderCreated);

            foreach (var order in _orderService.GetOrders())
            {

                if (order.CurrentStatus.Value == OrderStatuses.MiddlemanFinding)
                {
                    orders.Add(order);
                }
            }
            var ordersViewModel = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders);
            
            return View(ordersViewModel);
        }

        // GET: Middleman
        public ActionResult MyOrderList()
        {
            var orders = _orderService.GetOrders().Where(o => o.MiddlemanId == User.Identity.GetUserId());
            var ordersViewModel = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders);

            return View(ordersViewModel);
        }

        public ActionResult ProcessOrder(int? id)
        {
            if (id != null)
            {
                var order = _orderService.GetOrder(id.Value);
                if (order.CurrentStatus != null)
                {
                    if (order != null && order.CurrentStatus.Value == OrderStatuses.MiddlemanFinding)
                    {
                        order.StatusLogs.AddLast(new StatusLog()
                        {
                            OldStatus = order.CurrentStatus,
                            NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.SellerProviding),
                            TimeStamp = DateTime.Now
                        });
                        order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.SellerProviding);


                        order.BuyerChecked = false;
                        order.SellerChecked = false;
                        order.MiddlemanId = User.Identity.GetUserId();
                        
                        if (order.JobId != null)
                        {
                            BackgroundJob.Delete(order.JobId);
                            order.JobId = null;
                        }

                        _orderService.SaveOrder();

                        order.JobId = MarketHangfire.SetOrderCloseJob(order.Id, TimeSpan.FromDays(1));

                        _orderService.SaveOrder();
                        return RedirectToAction("MyOrderList");


                    }
                }


            }

            return HttpNotFound("fewfe");
        }

        // Provide data from moderator to buyer
        [HttpGet]
        public ActionResult ProvideDataToBuyer(int? id)
        {
            if (id != null)
            {
                var order = _orderService.GetOrder(id.Value);
                if (order != null)
                {
                    var accInfo = order.AccountInfo;
                    var model = Mapper.Map<AccountInfo, AccountInfoViewModel>(accInfo);
                    model.SellerId = order.SellerId;
                    model.BuyerId = order.BuyerId;
                    model.ModeratorId = order.MiddlemanId;
                    return View(model);
                }
            }
            return HttpNotFound();
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

                    var buyerOrder = _orderService.GetOrder(model.SteamLogin, model.ModeratorId, model.SellerId, model.BuyerId);
                    if (buyerOrder != null)
                    {
                        if (buyerOrder.CurrentStatus != null)
                        {
                            if (buyerOrder.CurrentStatus.Value == OrderStatuses.MidddlemanChecking)
                            {
                                buyerOrder.StatusLogs.AddLast(new StatusLog()
                                {
                                    OldStatus = buyerOrder.CurrentStatus,
                                    NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerConfirming),
                                    TimeStamp = DateTime.Now
                                });
                                buyerOrder.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerConfirming);


                                buyerOrder.BuyerChecked = false;
                                buyerOrder.SellerChecked = false;


                                _accountInfoService.UpdateAccountInfo(accInfo);
                                buyerOrder.AccountInfo = accInfo;
                                

                                if (buyerOrder.JobId != null)
                                {
                                    BackgroundJob.Delete(buyerOrder.JobId);
                                    buyerOrder.JobId = null;
                                }
                                _orderService.SaveOrder();

                                buyerOrder.JobId = MarketHangfire.SetConfirmOrderJob(buyerOrder.Id, TimeSpan.FromDays(2));

                                _orderService.SaveOrder();

                                return RedirectToAction("ProvideDataToBuyer", new { orderId = buyerOrder.Id });
                            }
                        }
                    }
                }
            }

            return HttpNotFound();
        }
    }
}