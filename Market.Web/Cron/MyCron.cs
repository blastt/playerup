using CronNET;
using Market.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.Cron
{
    public class MyCron : BaseJob
    {
        public MyCron()
        {

        }
        private readonly IOrderService _orderService;
        public MyCron(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public override CronExpression Cron
        {
            get { return CronExpression.EveryMinute; }
        }

        public override void Execute()
        {
            var order = _orderService.GetOrders().FirstOrDefault();
            order.Sum = 888;
            _orderService.SaveOrder();
        }
    }
}