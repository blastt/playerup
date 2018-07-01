using Market.Service;

namespace Market.Web.Hangfire
{
    public class OrderCloserConfiguration
    {
        public IOrderService OrderService { get; set; }
    }
}