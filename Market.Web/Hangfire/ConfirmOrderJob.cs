using Market.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.Hangfire
{
    public class ConfirmOrderJob : IJob
    {
        private readonly IOrderService orderService;

        public ConfirmOrderJob(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public void Do(int orderId)
        {
            var order = orderService.GetOrder(orderId);
            if (order != null)
            {
                orderService.ConfirmOrder(orderId, order.BuyerId);
                orderService.SaveOrder();
            }
        }
    }
}