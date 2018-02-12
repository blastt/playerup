using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class OrderViewModel
    {
        public Status OrderStatus { get; set; }
        public bool IsFeedbacked { get; set; }

        public string OfferName { get; set; }
        public string OfferId { get; set; }
        public string OfferUserName { get; set; }
        public string OrderUserName { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;
    }
}