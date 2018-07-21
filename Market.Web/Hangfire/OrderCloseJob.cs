using Hangfire;
using Market.Service;

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

        [DisableConcurrentExecution(10 * 60)]
        public void Do(int orderId)
        {           
            orderService.CloseOrderAutomatically(orderId);
            orderService.SaveOrderAsync();
                               
        }        
    }
}