﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class JsonFilter
    {
        public string Attribute { get; set; }
        public string Value { get; set; }
    }

    public class SearchViewModel
    {
        public int Page { get; set; } = 1;
        public string Sort { get; set; } = "bestSeller";
        public string[] Filters { get; set; }
        public bool IsOnline { get; set; }
        public bool SearchInDiscription { get; set; }
        public string SearchString { get; set; } = "";
        public IList<JsonFilter> JsonFilters { get; set; } = new List<JsonFilter>();
        public string[] FilterValues { get; set; }
        public string[] FilterItemValues { get; set; } = new string [] { };
        public string Game { get; set; } = "dota2";

        public decimal MinGamePrice { get; set; }

        public decimal MaxGamePrice { get; set; }

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