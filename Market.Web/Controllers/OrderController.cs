using AutoMapper;
using Hangfire;
using Market.Model.Models;
using Market.Service;
using Market.Web.Hangfire;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IOrderStatusService _orderStatusService;
        private readonly IStatusLogService _statusLogService;
        private readonly IOfferService _offerService;
        private readonly ITransactionService _transactionService;
        private readonly IOrderService _orderService;
        private readonly IGameService _gameService;
        public int pageSize = 1;
        public OrderController(IOfferService offerService, IStatusLogService statusLogService, IOrderStatusService orderStatusService, ITransactionService transactionService, IGameService gameService, IOrderService orderService, IUserProfileService userProfileService)
        {
            _orderStatusService = orderStatusService;
            _offerService = offerService;
            _gameService = gameService;
            _transactionService = transactionService;
            _orderService = orderService;
            _userProfileService = userProfileService;
            _statusLogService = statusLogService;
        }

        public ActionResult OrderBuy()
        {
            var currentUserId = User.Identity.GetUserId();
            var ordersBuy = _orderService.GetOrdersAsNoTracking(m => m.BuyerId == currentUserId, i => i.CurrentStatus, i => i.Offer, i => i.Seller);
            var ordersSell = _orderService.GetOrders().Where(m => m.SellerId == currentUserId);

            if (ordersBuy != null)
            {
                var orderViewModels = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(ordersBuy);
                ViewData["BuyCount"] = ordersBuy.Count();
                ViewData["SellCount"] = ordersSell.Count();
                var model = new OrderListViewModel
                {
                    Orders = orderViewModels.OrderByDescending(o => o.DateCreated)

                };
                

                return View(model);
            }

            return HttpNotFound();

        }

        public ActionResult OrderSell()
        {
            var currentUserId = User.Identity.GetUserId();
            var ordersBuy = _orderService.GetOrders().Where(m => m.BuyerId == currentUserId);
            var ordersSell = _orderService.GetOrdersAsNoTracking(m => m.SellerId == currentUserId, i => i.CurrentStatus, i => i.Offer, i => i.Buyer);

            if (ordersBuy != null)
            {
                var orderViewModels = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(ordersSell);
                ViewData["BuyCount"] = ordersBuy.Count();
                ViewData["SellCount"] = ordersSell.Count();
                var model = new OrderListViewModel
                {
                    Orders = orderViewModels.OrderByDescending(o => o.DateCreated)

                };

                return View(model);
            }

            return HttpNotFound();

        }

        public async Task<ActionResult> BuyDetails(int? Id)
        {
            if (Id != null)
            {

                var order = _orderService.GetOrder(Id.Value, i => i.Seller, i => i.Middleman, i => i.Buyer, i => i.CurrentStatus, i => i.StatusLogs, i => i.AccountInfos ,i => i.Offer , i => i.StatusLogs.Select(m => m.OldStatus), i => i.StatusLogs.Select(m => m.NewStatus));
                if (order != null)
                {
                    if (order.BuyerId == User.Identity.GetUserId())
                    {
                        order.BuyerChecked = true;
                        await _orderService.SaveOrderAsync();
                        var ordersBuy = _orderService.GetOrders().Where(m => m.BuyerId == User.Identity.GetUserId());
                        var ordersSell = _orderService.GetOrders().Where(m => m.SellerId == User.Identity.GetUserId());
                        ViewData["BuyCount"] = ordersBuy.Count();
                        ViewData["SellCount"] = ordersSell.Count();
                        DetailsOrderViewModel model = Mapper.Map<Order, DetailsOrderViewModel>(order);

                        var currentStatus = order.CurrentStatus.Value;
                        if (currentStatus == OrderStatuses.BuyerPaying ||
                            currentStatus == OrderStatuses.OrderCreating ||
                            currentStatus == OrderStatuses.MiddlemanFinding ||
                            currentStatus == OrderStatuses.SellerProviding ||
                            currentStatus == OrderStatuses.MidddlemanChecking)
                        {
                            model.ShowCloseButton = true;
                        }
                        if (currentStatus == OrderStatuses.BuyerPaying)
                        {
                            model.ShowPayButton = true;
                        }
                        if ((currentStatus == OrderStatuses.ClosedSuccessfully ||
                            currentStatus == OrderStatuses.PayingToSeller) && !order.BuyerFeedbacked)
                        {
                            model.ShowFeedbackToSeller = true;
                        }
                        if ((currentStatus == OrderStatuses.ClosedSuccessfully ||
                            currentStatus == OrderStatuses.PayingToSeller) && !order.SellerFeedbacked)
                        {
                            model.ShowFeedbackToBuyer = true;
                        }
                        if (currentStatus == OrderStatuses.BuyerConfirming ||
                            currentStatus == OrderStatuses.ClosedSuccessfully ||
                            currentStatus == OrderStatuses.PayingToSeller)
                        {
                            model.ShowAccountInfo = true;
                        }

                        if (currentStatus == OrderStatuses.BuyerConfirming)
                        {
                            model.ShowConfirm = true;
                        }

                        if (currentStatus == OrderStatuses.SellerProviding)
                        {
                            model.ShowProvideData = true;
                        }
                        if (order.Offer.SellerPaysMiddleman)
                        {
                            model.MiddlemanPrice = 0;
                        }
                        else
                        {
                            model.MiddlemanPrice = order.Offer.MiddlemanPrice.Value;
                        }
                        IList<StatusLog> orderLogs = new List<StatusLog>();

                        foreach (var log in order.StatusLogs)
                        {
                            orderLogs.Add(log);
                            if (log.NewStatus.Value == OrderStatuses.AbortedByBuyer)
                            {
                                var test = order.StatusLogs.FirstOrDefault(s => s.NewStatus.Value == OrderStatuses.BuyerConfirming);
                                orderLogs.Remove(test);
                            }
                        }

                        model.Logs = orderLogs;

                        model.CurrentStatusName = order.CurrentStatus.DuringName;
                        model.StatusLogs = order.StatusLogs;
                        model.ModeratorId = order.MiddlemanId;
                        return View(model);
                    }
                    return RedirectToAction("OrderBuy");
                }


            }

            return HttpNotFound();

        }

        public ActionResult Close(int? Id)
        {
            string userId = User.Identity.GetUserId();
            if (userId != null && Id != null)
            {
                var order = _orderService.GetOrder(Id.Value, i => i.Seller, i => i.Seller.ApplicationUser, i => i.Buyer.ApplicationUser, i => i.CurrentStatus, i => i.Buyer, i => i.Middleman, i => i.Seller);

                bool closeResult = false;
                if (order != null)
                {
                    if (userId == order.BuyerId)
                    {
                        closeResult = _orderService.CloseOrderByBuyer(order.Id);
                        if (closeResult)
                        {
                            if (order.JobId != null)
                            {
                                BackgroundJob.Delete(order.JobId);
                                order.JobId = null;
                            }
                            TempData["orderBuyStatus"] = "Сделка закрыта.";
                            _orderService.SaveOrder();
                            MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Seller.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("SellDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                            MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Buyer.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("BuyDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));
                            return RedirectToAction("BuyDetails", new { id = order.Id });
                        }
                    }
                    else if (userId == order.SellerId)
                    {
                        closeResult = _orderService.CloseOrderBySeller(order.Id);
                        if (closeResult)
                        {
                            if (order.JobId != null)
                            {
                                BackgroundJob.Delete(order.JobId);
                                order.JobId = null;
                            }
                            TempData["orderSellStatus"] = "Сделка закрыта.";
                            _orderService.SaveOrder();
                            MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Seller.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("SellDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                            MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Buyer.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("BuyDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));
                            return RedirectToAction("SellDetails", new { id = order.Id });
                        }
                    }
                    else if (userId == order.MiddlemanId)
                    {

                        closeResult = _orderService.CloseOrderByMiddleman(order.Id);
                        if (closeResult)
                        {
                            if (order.JobId != null)
                            {
                                BackgroundJob.Delete(order.JobId);

                                order.JobId = null;
                            }
                            _orderService.SaveOrder();
                            MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Seller.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("SellDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                            MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Buyer.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("BuyDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));
                        }
                    }

                    if (order.BuyerId == userId)
                    {
                        return RedirectToAction("BuyDetails", "Order", new { id = order.Id });
                    }
                    else if (order.SellerId == userId)
                    {
                        return RedirectToAction("SellDetails", "Order", new { id = order.Id });
                    }
                }
                
            }
            return HttpNotFound();
        }

        public ActionResult Abort(int? id)
        {
            if (id != null)
            {
                bool result = _orderService.AbortOrder(id.Value, User.Identity.GetUserId());
                var order = _orderService.GetOrder(id.Value, i => i.Seller, i => i.Buyer,
                    i => i.Buyer.ApplicationUser, i => i.Seller.ApplicationUser, i => i.CurrentStatus);
                if (result && order != null)
                {
                    if (order.JobId != null)
                    {
                        BackgroundJob.Delete(order.JobId);
                        order.JobId = null;
                    }

                    _orderService.SaveOrder();

                    MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Seller.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("SellDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                    MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Buyer.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("BuyDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                    _orderService.SaveOrder();
                    return RedirectToAction("BuyDetails", "Order", new { id });
                }
            }
            return HttpNotFound();
        }

        public ActionResult SellDetails(int? Id)
        {
            if (Id != null)
            {
                var order = _orderService.GetOrder(Id.Value, i => i.Seller, i => i.Middleman, i => i.CurrentStatus, i => i.Buyer, i => i.Offer, i => i.StatusLogs, i => i.StatusLogs.Select(m => m.OldStatus), i => i.StatusLogs.Select(m => m.NewStatus));
                if (order != null)
                {
                    if (order.SellerId == User.Identity.GetUserId())
                    {
                        order.SellerChecked = true;
                        _orderService.SaveOrder();
                        DetailsOrderViewModel model = Mapper.Map<Order, DetailsOrderViewModel>(order);
                        var ordersBuy = _orderService.GetOrders().Where(m => m.BuyerId == User.Identity.GetUserId());
                        var ordersSell = _orderService.GetOrders().Where(m => m.SellerId == User.Identity.GetUserId());
                        ViewData["BuyCount"] = ordersBuy.Count();
                        ViewData["SellCount"] = ordersSell.Count();
                        var currentStatus = order.CurrentStatus.Value;
                        if (currentStatus == OrderStatuses.BuyerPaying ||
                            currentStatus == OrderStatuses.OrderCreating ||
                            currentStatus == OrderStatuses.MiddlemanFinding ||
                            currentStatus == OrderStatuses.SellerProviding ||
                            currentStatus == OrderStatuses.MidddlemanChecking)
                        {
                            model.ShowCloseButton = true;
                        }
                        if (currentStatus == OrderStatuses.BuyerPaying)
                        {
                            model.ShowPayButton = true;
                        }
                        if ((currentStatus == OrderStatuses.ClosedSuccessfully ||
                            currentStatus == OrderStatuses.PayingToSeller) && !order.BuyerFeedbacked)
                        {
                            model.ShowFeedbackToSeller = true;
                        }
                        if ((currentStatus == OrderStatuses.ClosedSuccessfully ||
                            currentStatus == OrderStatuses.PayingToSeller) && !order.SellerFeedbacked)
                        {
                            model.ShowFeedbackToBuyer = true;
                        }
                        if (currentStatus == OrderStatuses.BuyerConfirming ||
                            currentStatus == OrderStatuses.ClosedSuccessfully ||
                            currentStatus == OrderStatuses.PayingToSeller)
                        {
                            model.ShowAccountInfo = true;
                        }

                        if (currentStatus == OrderStatuses.BuyerConfirming)
                        {
                            model.ShowConfirm = true;
                        }

                        if (currentStatus == OrderStatuses.SellerProviding)
                        {
                            model.ShowProvideData = true;
                        }

                        IList<StatusLog> orderLogs = new List<StatusLog>();

                        foreach (var log in order.StatusLogs)
                        {
                            orderLogs.Add(log);
                            if (log.OldStatus.Value == OrderStatuses.AbortedByBuyer)
                            {
                                var test = order.StatusLogs.FirstOrDefault(s => s.OldStatus.Value == OrderStatuses.BuyerConfirming);
                                orderLogs.Remove(test);
                            }
                            
                        }
                        var lastLog = orderLogs.LastOrDefault();
                        if (lastLog != null)
                        {
                            if (lastLog.NewStatus.Value == OrderStatuses.ClosedAutomatically || lastLog.NewStatus.Value == OrderStatuses.BuyerClosed ||
                                lastLog.NewStatus.Value == OrderStatuses.MiddlemanClosed || lastLog.NewStatus.Value == OrderStatuses.SellerClosed)
                            {
                                orderLogs.Remove(lastLog);
                            }

                            
                        }
                        model.Logs = orderLogs;

                        model.CurrentStatusName = order.CurrentStatus.DuringName;

                        model.StatusLogs = order.StatusLogs;
                        model.ModeratorId = order.MiddlemanId;
                        return View(model);
                    }
                    return RedirectToAction("OrderBuy");
                }

            }

            return HttpNotFound();

        }

        public string GetOrdersCount()
        {
            string currentUserId = User.Identity.GetUserId();
            int ordersCount = 0;
            string result = null;
            var ordersBuyer = _orderService.GetOrders(o => o.BuyerId == currentUserId && !o.BuyerChecked, i => i.Buyer).ToList();
            var ordersSeller = _orderService.GetOrders(o => o.SellerId == currentUserId && !o.SellerChecked, i => i.Seller).ToList();
            if (ordersBuyer != null && ordersSeller != null)
            {
                ordersCount = ordersBuyer.Count() + ordersSeller.Count();
            }
            if (ordersCount != 0)
            {
                result = ordersCount.ToString();
            }
            
            
            return result;
        }
    }
}