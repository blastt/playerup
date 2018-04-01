using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public ICollection<OrderStatus> OrderStatuses { get; set; } = new List<OrderStatus>();
        //public bool IsFeedbacked { get; set; }

        public bool BuyerChecked { get; set; }
        public bool SellerChecked { get; set; }

        public string OfferHeader { get; set; }
        public decimal OfferPrice { get; set; }
        public string OfferId { get; set; }
        public string BuyerName { get; set; }
        public string SellerName { get; set; }

        public DateTime DateCreated { get; set; }
    }
}