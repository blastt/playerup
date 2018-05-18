using Market.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Market.Web.Hangfire
{
    public class ConfirmOrderJob
    {
        private readonly IOrderService orderService;

        public ConfirmOrderJob(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task Do(int orderId)
        {
            var order = await orderService.GetOrderAsync(orderId);
            
            if (order != null)
            {
                var result = orderService.ConfirmOrder(orderId, order.BuyerId);
                if (result)
                {
                    order.JobId = MarketHangfire.SetLeaveFeedbackJob(order.SellerId, order.BuyerId, order.Id, TimeSpan.FromDays(15));
                }
                await orderService.SaveOrderAsync();
            }
        }
    }
}