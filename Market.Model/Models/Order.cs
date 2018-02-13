using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public enum Status
    {
        Created,
        BuyerCancalledEarly,
        SellerCancalledEarly,
        ModeratorCancalledEarly,
        Successfully
    }
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public Status OrderStatus { get; set; }
        public bool IsFeedbacked { get; set; }

        public virtual Offer Offer { get; set; }
        public virtual string UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;
    }
}
