using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class DetailsOrderViewModel
    {
        public int Id { get; set; }
        public Status OrderStatus { get; set; }
        //public bool IsFeedbacked { get; set; }

        public string OfferHeader { get; set; }
        public decimal OfferPrice { get; set; }
        public string OfferId { get; set; }

        public string ModeratorId { get; set; }
        public string ModeratorName { get; set; }

        public bool SellerFeedbacked { get; set; }
        public bool BuyerFeedbacked { get; set; }

        public string BuyerId { get; set; }
        public string BuyerName { get; set; }

        public string SellerId { get; set; }
        public string SellerName { get; set; }

        public DateTime DateCreated { get; set; }
    }
}