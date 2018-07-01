using System.Collections.Generic;

namespace Market.Model.Models
{
    public enum OrderStatuses
    {
        OrderCreating,
        BuyerPaying,
        MiddlemanFinding,
        SellerProviding,
        MidddlemanChecking,
        BuyerConfirming,
        PayingToSeller,
        Feedbacking,
        ClosedSuccessfully,
        BuyerClosed,
        SellerClosed,
        MiddlemanClosed,
        ClosedAutomatically,
        AbortedByBuyer,
        MiddlemanBackingAccount
    }

    public class OrderStatus
    {
        public int Id { get; set; }
        public string DuringName { get; set; }
        public string FinishedName { get; set; }
        public OrderStatuses Value { get; set; }


        public IList<Order> Orders { get; set; }

        public IList<StatusLog> NewStatusLogs { get; set; }
        public IList<StatusLog> OldStatusLogs { get; set; }
    }
}
