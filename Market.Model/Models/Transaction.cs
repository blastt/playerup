using System;

namespace Market.Model.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public string SenderId { get; set; }
        public UserProfile Sender { get; set; }

        public string ReceiverId { get; set; }
        public UserProfile Receiver { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public DateTime TransactionDate { get; set; }

    }
}
