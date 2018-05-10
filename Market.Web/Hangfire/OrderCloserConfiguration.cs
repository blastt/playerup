using Market.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.Hangfire
{
    public class OrderCloserConfiguration
    {
        public IOrderService OrderService { get; set; }
    }
}