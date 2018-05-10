using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        ClosedAutomatically
    }

    public class OrderStatus
    {
        public int Id { get; set; }
        public string DuringName { get; set; }
        public string FinishedName { get; set; }
        public OrderStatuses Value { get; set; }


        public virtual IList<Order> Orders { get; set; }

        public virtual IList<StatusLog> NewStatusLogs { get; set; }
        public virtual IList<StatusLog> OldStatusLogs { get; set; }
    }
}
