﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class OfferViewModel
    {
        public string Header { get; set; }
        public string Game { get; set; }
        public string Filter { get; set; }
        public decimal Price { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public string UserName { get; set; }
    }
}