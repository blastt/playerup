using Market.Model.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Web.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public OrderStatus CurrentStatus { get; set; }
        //public bool IsFeedbacked { get; set; }

        public bool BuyerChecked { get; set; }
        public bool SellerChecked { get; set; }

        public string OfferHeader { get; set; }

        [DataType(DataType.Currency)]
        public decimal OfferPrice { get; set; }
        public string OfferId { get; set; }
        public string BuyerName { get; set; }
        public string SellerName { get; set; }

        public DateTime DateCreated { get; set; }
    }
}