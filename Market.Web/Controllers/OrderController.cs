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
    public class OrderController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IOrderStatusService _orderStatusService;
        private readonly IOfferService _offerService;
        private readonly IOrderService _orderService;
        private readonly IGameService _gameService;
        public int pageSize = 5;
        public OrderController(IOfferService offerService, IOrderStatusService orderStatusService, IGameService gameService, IOrderService orderService, IUserProfileService userProfileService)
        {
            _orderStatusService = orderStatusService;
            _offerService = offerService;
            _gameService = gameService;
            _orderService = orderService;
            _userProfileService = userProfileService;
        }

        public ActionResult OrderBuy()
        {
            var ordersBuy = _orderService.GetOrders().Where(m => m.BuyerId == User.Identity.GetUserId());
            var ordersSell = _orderService.GetOrders().Where(m => m.SellerId == User.Identity.GetUserId());

            if (ordersBuy != null)
            {
                var orderViewModels = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(ordersBuy);

                var model = new OrderListViewModel
                {
                    Orders = orderViewModels,
                    BuyCount = ordersBuy.Count(),
                    SellCount = ordersSell.Count()

                };

                return View(model);
            }

            return HttpNotFound();

        }

        public ActionResult OrderSell()
        {
            var ordersBuy = _orderService.GetOrders().Where(m => m.BuyerId == User.Identity.GetUserId());
            var ordersSell = _orderService.GetOrders().Where(m => m.SellerId == User.Identity.GetUserId());

            if (ordersBuy != null)
            {
                var orderViewModels = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(ordersSell);

                var model = new OrderListViewModel
                {
                    Orders = orderViewModels,
                    BuyCount = ordersBuy.Count(),
                    SellCount = ordersSell.Count()

                };

                return View(model);
            }

            return HttpNotFound();

        }

        public ActionResult BuyDetails(int? orderId)
        {
            if (orderId != null)
            {

                var order = _orderService.GetOrder(orderId.Value);
                if (order != null)
                {
                    if (order.BuyerId == User.Identity.GetUserId())
                    {
                        order.BuyerChecked = true;
                        _orderService.SaveOrder();
                        DetailsOrderViewModel model = Mapper.Map<Order, DetailsOrderViewModel>(order);
                        
                        var currentStatus = order.CurrentStatus.Value;
                        if (currentStatus == OrderStatuses.BuyerPaying || 
                            currentStatus == OrderStatuses.OrderCreating || 
                            currentStatus == OrderStatuses.MiddlemanFinding|| 
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
                        model.CurrentStatusName = order.CurrentStatus.DuringName;
                        model.StatusLogs = order.StatusLogs;
                        model.ModeratorId = order.MiddlemanId;
                        return View(model);
                    }

                }


            }

            return HttpNotFound();

        }

        public ActionResult Close(int? orderId)
        {
            string userId = User.Identity.GetUserId();
            if (userId != null && orderId != null)
            {
                var order = _orderService.GetOrder(orderId.Value);
                if (order.CurrentStatus.Value == OrderStatuses.BuyerPaying ||
                    order.CurrentStatus.Value == OrderStatuses.OrderCreating ||
                    order.CurrentStatus.Value == OrderStatuses.MiddlemanFinding ||
                        order.CurrentStatus.Value == OrderStatuses.SellerProviding ||
                    order.CurrentStatus.Value == OrderStatuses.MidddlemanChecking)
                {
                    if (userId == order.BuyerId)
                    {
                        var orderStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerClosed);
                        if (orderStatus != null)
                        {
                            order.CurrentStatus = orderStatus;
                            _orderService.SaveOrder();
                            return View();
                        }                                               
                    }
                    else if (userId == order.SellerId)
                    {
                        var orderStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.SellerClosed);
                        if (orderStatus != null)
                        {
                            order.CurrentStatus = orderStatus;
                            _orderService.SaveOrder();
                            return View();
                        }
                    }
                    else if (userId == order.MiddlemanId)
                    {
                        var orderStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanClosed);
                        if (orderStatus != null)
                        {
                            order.CurrentStatus = orderStatus;
                            _orderService.SaveOrder();
                            return View();
                        }
                    }
                }

            }
            return HttpNotFound();

        }

        public ActionResult SellDetails(int? orderId)
        {
            if (orderId != null)
            {
                var order = _orderService.GetOrder(orderId.Value);
                if (order != null)
                {
                    if (order.SellerId == User.Identity.GetUserId())
                    {
                        order.BuyerChecked = true;
                        _orderService.SaveOrder();
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
                        model.CurrentStatusName = order.CurrentStatus.DuringName;
                        model.StatusLogs = order.StatusLogs;
                        model.ModeratorId = order.MiddlemanId;
                        return View(model);
                    }

                }

            }

            return HttpNotFound();

        }

        public JsonResult GetOrdersCount()
        {
            string currentUserId = User.Identity.GetUserId();
            int ordersCount = 0;
            var ordersBuyer = _orderService.GetOrders().Where(o => o.BuyerId == currentUserId && !o.BuyerChecked);
            var ordersSeller = _orderService.GetOrders().Where(o => o.SellerId == currentUserId && !o.SellerChecked);
            if (ordersBuyer != null && ordersSeller != null)
            {
                ordersCount = ordersBuyer.Count() + ordersSeller.Count();
            }

            return Json(ordersCount);
        }
    }
}