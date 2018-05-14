using Hangfire;
using Market.Model.Models;
using Market.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.Hangfire
{
    //[AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public class OrderCloseJob
    {
        private readonly IOrderService orderService;

        public OrderCloseJob(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public void Do(int orderId)
        {           
            orderService.CloseOrderAutomatically(orderId);
            orderService.SaveOrder();
                               
        }        
    }
}