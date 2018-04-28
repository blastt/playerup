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
        public IEnumerable<StatusLog> StatusLogs { get; set; }
        //public bool IsFeedbacked { get; set; }


        public string CurrentStatusName { get; set; }
        public string OfferHeader { get; set; }
        public decimal OfferPrice { get; set; }
        public string OfferId { get; set; }

        public bool ShowPayButton { get; set; }
        public bool ShowCloseButton { get; set; }
        public bool ShowFeedbackToBuyer { get; set; }
        public bool ShowFeedbackToSeller { get; set; }
        public bool ShowAccountInfo { get; set; }
        public bool ShowConfirm { get; set; }
        public bool ShowProvideData { get; set; }

        public string ModeratorId { get; set; }
        public string ModeratorName { get; set; }

        public bool SellerFeedbacked { get; set; }
        public bool BuyerFeedbacked { get; set; }

        public string BuyerId { get; set; }
        public string BuyerName { get; set; }

        public string SellerId { get; set; }
        public string SellerName { get; set; }

        public AccountInfo AccountInfo { get; set; }

        public DateTime DateCreated { get; set; }
    }
}