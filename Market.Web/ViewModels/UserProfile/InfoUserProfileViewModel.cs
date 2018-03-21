﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class InfoUserProfileViewModel
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public int PositiveFeedbacks { get; set; }
        public int NegativeFeedbacks { get; set; }
        public double PositiveFeedbackProcent { get; set; }
        public double NegativeFeedbackProcent { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsOnline { get; set; }
        public int SoldItems { get; set; }
        public int ReturnedItems { get; set; }
        public int Rating { get; set; }
        public string CurrentUserId { get; set; }

        public OfferListViewModel OffersViewModel { get; set; } = new OfferListViewModel();
        public FeedbackListViewModel FeedbacksViewModel { get; set; } = new FeedbackListViewModel();
    }
}