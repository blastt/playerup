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
                var result = orderService.ConfirmOrder(orderId, order.BuyerId);
                if (result)
                {
                    order.JobId = MarketHangfire.SetLeaveFeedbackJob(order.SellerId, order.BuyerId, order.Id, TimeSpan.FromDays(15));
                }
                orderService.SaveOrder();
            }
        }
    }
}