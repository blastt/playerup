﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class SearchViewModel
    {
        public int Page { get; set; }
        public string Sort { get; set; }
        public bool IsOnline { get; set; }
        public string SearchString { get; set; }
        public string[] FilterItems { get; set; }
        public string Game { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        
    }
     //"page": "p",
     //"sort": "sort",
     //"online": "sort",
     //"search": "sort",
     //"filterItems": "sort",
     //"game": "sort",
     //"priceFrom": "sort",
     //"priceTo": "sort"
}