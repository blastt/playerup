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
        public ActionResult OrderTransactionDetails(int? id)
        {
            if (id != null)
            {
                var order = _orderService.GetOrder(id.Value, i => i.Transactions, i => i.Offer, 
                    i => i.Transactions.Select(t => t.Sender), i => i.Transactions.Select(t => t.Receiver));
                IList<OrderTransactionDetails> model = new List<OrderTransactionDetails>();
                if (order != null)
                {
                    foreach (var transaction in order.Transactions)
                    {
                        var transModel = Mapper.Map<Transaction, OrderTransactionDetails>(transaction);
                        //TODO
                        if (transaction.Receiver.Name == "palyerup")
                        {
                            transModel.ReceiverName = "PLAYER UP";
                        }
                        if (transaction.Sender.Name == "palyerup")
                        {
                            transModel.SenderName = "PLAYER UP";
                        }
                        transModel.OrderHeader = order.Offer.Header;
                        transModel.OrderId = order.Id;
                        model.Add(transModel);
                    }
                }
                return View(model.AsEnumerable());
            }
            return HttpNotFound();
        }

        // GET: Middleman
        public ActionResult OrderList()
        {
            var orders = _orderService.GetOrdersAsNoTracking(o => o.CurrentStatus.Value == OrderStatuses.MiddlemanFinding, i => i.CurrentStatus, i => i.Offer, i => i.Seller, i => i.Buyer).ToList();//.Where(o => o.OrderStatus == Status.OrderCreated);
            
            var ordersViewModel = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders);

            return View(ordersViewModel);
        }

        // GET: Middleman
        public ActionResult MyOrderList()
        {
            var currentUserId = User.Identity.GetUserId();
            var orders = _orderService.GetOrdersAsNoTracking(o => o.MiddlemanId == currentUserId, i => i.CurrentStatus, i => i.Offer, i => i.Seller, i => i.Buyer).ToList();
            var ordersViewModel = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders);

            return View(ordersViewModel);
        }

        public ActionResult OrderAccessDetails(int? id)
        {
            if (id != null)
            {
                var order = _orderService.GetOrder(id.Value, i => i.AccountInfos, i => i.Middleman);
                if (order != null)
                {
                    if (order.MiddlemanId == User.Identity.GetUserId())
                    {
                        var model = Mapper.Map<IEnumerable<AccountInfo>, IEnumerable<AccountInfoViewModel>>(order.AccountInfos);
                        return View(model);
                    }
                }
            }
            return HttpNotFound();
        }

        public ActionResult ProcessOrder(int? id)
        {
            if (id != null)
            {
                var order = _orderService.GetOrder(id.Value, i => i.CurrentStatus, i => i.Seller.ApplicationUser, i => i.Buyer.ApplicationUser, i => i.StatusLogs);
                if (order.CurrentStatus != null)
                {
                    if (order.CurrentStatus.Value == OrderStatuses.MiddlemanFinding)
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


                        if (Request.Url != null)
                        {
                            MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Seller.ApplicationUser.Email,
                                order.CurrentStatus.DuringName,
                                Url.Action("SellDetails", "Order", new {id = order.Id}, protocol: Request.Url.Scheme));

                            MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Buyer.ApplicationUser.Email,
                                order.CurrentStatus.DuringName,
                                Url.Action("BuyDetails", "Order", new {id = order.Id}, protocol: Request.Url.Scheme));
                        }

                        _orderService.SaveOrder();
                        return RedirectToAction("MyOrderList");


                    }
                }


            }

            return HttpNotFound();
        }

        // Provide data from moderator to buyer
        [HttpGet]
        public ActionResult ProvideDataToBuyer(int? id)
        {
            if (id != null)
            {
                var order = _orderService.GetOrder(id.Value, i => i.AccountInfos, i => i.Seller, i => i.Buyer, i => i.Middleman);
                if (order != null)
                {
                    var accInfo = order.AccountInfos.Last();
                    var model = Mapper.Map<AccountInfo, AccountInfoViewModel>(accInfo);
                    model.SellerId = order.SellerId;
                    model.BuyerId = order.BuyerId;
                    model.ModeratorId = order.MiddlemanId;
                    model.OrderId = order.Id;
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
                var accountInfo = Mapper.Map<AccountInfoViewModel, AccountInfo>(model);
                var buyerOrder = _orderService.GetOrder(model.SteamLogin, model.ModeratorId, model.SellerId, model.BuyerId, i => i.CurrentStatus, 
                    i => i.StatusLogs, i => i.AccountInfos, i => i.Seller.ApplicationUser, i => i.Buyer.ApplicationUser);
                if (buyerOrder?.CurrentStatus != null)
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


                        buyerOrder.AccountInfos.Add(accountInfo);
                        _accountInfoService.CreateAccountInfo(accountInfo);

                        if (buyerOrder.JobId != null)
                        {
                            BackgroundJob.Delete(buyerOrder.JobId);
                            buyerOrder.JobId = null;
                        }
                        _orderService.SaveOrder();

                        buyerOrder.JobId = MarketHangfire.SetConfirmOrderJob(buyerOrder.Id, TimeSpan.FromDays(2));


                        if (Request.Url != null)
                        {
                            MarketHangfire.SetSendEmailChangeStatus(buyerOrder.Id,
                                buyerOrder.Seller.ApplicationUser.Email, buyerOrder.CurrentStatus.DuringName,
                                Url.Action("SellDetails", "Order", new {id = buyerOrder.Id},
                                    protocol: Request.Url.Scheme));

                            MarketHangfire.SetSendEmailAccountData(accountInfo.Login, accountInfo.Password, accountInfo.Email, accountInfo.EmailPassword, 
                                accountInfo.AdditionalInformation, buyerOrder.Buyer.ApplicationUser.Email);
                        }

                        _orderService.SaveOrder();

                        return RedirectToAction("ProvideDataToBuyer", new { orderId = buyerOrder.Id });
                    }
                }
            }
            return HttpNotFound();
        }


        public ActionResult ConfirmAbortOrder(int? id)
        {
            if (id != null)
            {
                bool result = _orderService.ConfirmAbortOrder(id.Value, User.Identity.GetUserId());
                var order = _orderService.GetOrder(id.Value, i => i.Seller.ApplicationUser, i => i.Buyer.ApplicationUser, i => i.CurrentStatus);
                if (result && order != null)
                {
                    if (order.JobId != null)
                    {
                        BackgroundJob.Delete(order.JobId);
                        order.JobId = null;
                    }

                    _orderService.SaveOrder();

                    if (Request.Url != null)
                    {
                        MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Seller.ApplicationUser.Email,
                            order.CurrentStatus.DuringName,
                            Url.Action("SellDetails", "Order", new {id = order.Id}, protocol: Request.Url.Scheme));

                        MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Buyer.ApplicationUser.Email,
                            order.CurrentStatus.DuringName,
                            Url.Action("BuyDetails", "Order", new {id = order.Id}, protocol: Request.Url.Scheme));
                    }

                    _orderService.SaveOrder();
                    return RedirectToAction("BuyDetails", "Order", new { id });
                }
            }
            return HttpNotFound();
        }

        public ActionResult CloseOrderSuccess(int? id)
        {
            string userId = User.Identity.GetUserId();
            if (userId != null && id != null)
            {
                var order = _orderService.GetOrder(id.Value, i => i.Middleman);
                if (order != null)
                {
                    if (userId == order.MiddlemanId)
                    {
                        var closeResult = _orderService.ConfirmOrderByMiddleman(order.Id, userId);
                        if (closeResult)
                        {

                            if (order.JobId != null)
                            {
                                BackgroundJob.Delete(order.JobId);
                                order.JobId = null;
                            }


                            TempData["orderBuyStatus"] = "Сделка закрыта.";
                            _orderService.SaveOrder();
                            return RedirectToAction("MyOrderList");
                        }
                    }
                }

            }
            return HttpNotFound();
        }

    }
}