using Hangfire;
using Market.Service;
using System;
using System.Threading.Tasks;

namespace Market.Web.Hangfire
{
    public class ConfirmOrderJob
    {
        private readonly IOrderService orderService;

        public ConfirmOrderJob(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [DisableConcurrentExecution(10 * 60)]
        public async Task Do(int orderId)
        {
            var order = orderService.GetOrder(orderId, i => i.BuyerId, i => i.SellerId);
            
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