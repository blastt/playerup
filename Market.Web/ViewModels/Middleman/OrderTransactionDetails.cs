using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class OrderTransactionDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderHeader { get; set; }
        public string SenderName { get; set; }

        public string ReceiverName { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}