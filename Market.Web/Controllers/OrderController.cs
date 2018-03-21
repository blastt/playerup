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
        private readonly IOfferService _offerService;
        private readonly IOrderService _orderService;
        private readonly IGameService _gameService;
        public int pageSize = 5;
        public OrderController(IOfferService offerService, IGameService gameService, IOrderService orderService, IUserProfileService userProfileService)
        {
            _offerService = offerService;
            _gameService = gameService;
            _orderService = orderService;
            _userProfileService = userProfileService;
        }

        public ActionResult OrderBuy()
        {
            var orders = _orderService.GetOrders().Where(m => m.BuyerId == User.Identity.GetUserId());

            if(orders != null)
            {
                var orderViewModels = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders);

                var model = new OrderListViewModel
                {
                    Orders = orderViewModels

                };

                return View(model);
            }

            return HttpNotFound("Lol");
            
        }

        public ActionResult OrderSell()
        {
            var orders = _orderService.GetOrders().Where(m => m.SellerId == User.Identity.GetUserId());

            if (orders != null)
            {
                var orderViewModels = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders);

                var model = new OrderListViewModel
                {
                    Orders = orderViewModels

                };

                return View(model);
            }

            return HttpNotFound("Lol");

        }

        public ActionResult BuyDetails(int? orderId)
        {
            if(orderId != null)
            {
                var order = _orderService.GetOrder(orderId.Value);
                order.BuyerChecked = true;
                _orderService.SaveOrder();
                DetailsOrderViewModel model = Mapper.Map<Order, DetailsOrderViewModel>(order);
                model.ModeratorId = order.ModeratorId;
                return View(model);
            }
            
            return HttpNotFound("Lol");

        }

        public ActionResult SellDetails(int? orderId)
        {
            if (orderId != null)
            {
                var order = _orderService.GetOrder(orderId.Value);
                order.SellerChecked = true;
                _orderService.SaveOrder();
                DetailsOrderViewModel model = Mapper.Map<Order, DetailsOrderViewModel>(order);
                model.ModeratorId = order.ModeratorId;
                return View(model);
            }

            return HttpNotFound("Lol");

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