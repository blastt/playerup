﻿using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Web.ViewModels
{
    public class DetailsOrderViewModel
    {
        public int Id { get; set; }
        public IEnumerable<StatusLog> StatusLogs { get; set; }
        //public bool IsFeedbacked { get; set; }


        public string CurrentStatusName { get; set; }
        public string OfferHeader { get; set; }

        [DataType(DataType.Currency)]
        public decimal OfferPrice { get; set; }
        public string OfferId { get; set; }

        [DataType(DataType.Currency)]
        public decimal MiddlemanPrice { get; set; }
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

        public IList<StatusLog> Logs { get; set; } = new List<StatusLog>();

        public virtual IList<AccountInfo> AccountInfos { get; set; } = new List<AccountInfo>();

        public DateTime DateCreated { get; set; }
    }
}