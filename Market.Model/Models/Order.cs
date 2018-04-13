using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public enum Status
    {
        
        OrderCreated,
        SellerProviding,
        AdminChecking,
        BuyerConfirming,        
        PayingToSeller,
        ClosedSeccessfuly,
        ColsedFelure,
        BuyerCancelledEarly,
        SellerCancelledEarly,
        AdminCancelledEarly

    }
    public class Order
    {
        public int Id { get; set; }
        public bool BuyerFeedbacked { get; set; }
        public bool SellerFeedbacked { get; set; }

        public bool BuyerChecked { get; set; }
        public bool SellerChecked { get; set; }

        public decimal Sum { get; set; } // сумма заказа
        public decimal? Amount { get; set; } // сумма, которую заплатали с учетом комиссии
        public decimal? WithdrawAmount { get; set; } // сумма, которую заплатали без учета комиссии

        public virtual Feedback Feedback { get; set; }

        public virtual int OfferId { get; set; }
        public virtual Offer Offer { get; set; }

        public virtual ICollection<OrderStatus> OrderStatuses { get; set; } = new List<OrderStatus>();

        public virtual string MiddlemanId { get; set; }
        public virtual UserProfile Middleman { get; set; }
        public virtual string BuyerId { get; set; }
        public virtual UserProfile Buyer { get; set; }
        public virtual string SellerId { get; set; }
        public virtual UserProfile Seller { get; set; }

        public virtual AccountInfo AccountInfo { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
