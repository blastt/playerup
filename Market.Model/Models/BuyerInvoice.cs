using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class BuyerInvoice
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePay { get; set; }

        public virtual string UserId { get; set; }
        public virtual UserProfile User { get; set; }

        public virtual int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
