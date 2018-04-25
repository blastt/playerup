using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class CheckoutViewModel
    {
        public int OfferId { get; set; }
        public int Quantity { get; set; }
        public string Game { get; set; }
        public string OfferHeader { get; set; }

        [DataType(DataType.Currency)]
        public decimal OrderSum { get; set; }
        public string SellerId { get; set; }
    }
}