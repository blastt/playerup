using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public virtual string SenderId { get; set; }
        public virtual UserProfile Sender { get; set; }

        public virtual string ReceiverId { get; set; }
        public virtual UserProfile Receiver { get; set; }

        public virtual int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public DateTime TransactionDate { get; set; }

    }
}
