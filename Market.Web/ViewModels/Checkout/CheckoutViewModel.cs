using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels.Checkout
{
    public class CheckoutViewModel
    {
        public int OfferId { get; set; }
        public string OfferHeader { get; set; }
        public string SellerId { get; set; }
        public string sellerName { get; set; }
        public string BuyerId { get; set; }
        public string BuyerName { get; set; }
    }
}