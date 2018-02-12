﻿using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class FeedbackViewModel
    {
        public Emotions Grade { get; set; }
        public string Comment { get; set; }
        public DateTime? DateLeft { get; set; }

        public string OfferHeader { get; set; }
    }
}