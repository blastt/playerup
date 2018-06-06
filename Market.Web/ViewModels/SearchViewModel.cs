using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.ViewModels
{
    public class JsonFilter
    {
        public string Attribute { get; set; }
        public string Value { get; set; }
    }
    public class SelectFilter
    {
        public string FilterValue { get; set; }
        public string FilterName { get; set; }
        public IList<FilterItemViewModel> Options { get; set; } = new List<FilterItemViewModel>();
    }

    public class SearchViewModel
    {
        public int Page { get; set; } = 1;
        public string Sort { get; set; } = "bestSeller";
        public IEnumerable<GameViewModel> Games { get; set; }
        public string[] Filters { get; set; } = new string[] { };
        public bool IsOnline { get; set; }
        public bool SearchInDiscription { get; set; }
        public string SearchString { get; set; } = "";
        public IList<JsonFilter> JsonFilters { get; set; } = new List<JsonFilter>();

        public IList<FilterItemViewModel> FilterValuesArr { get; set; } = new List<FilterItemViewModel>();
        public IList<SelectListItem> SortItems { get; set; } = new List<SelectListItem>();
        public IList<SelectFilter> SelectsOptions { get; set; } = new List<SelectFilter>();

        public string[] FilterValues { get; set; }
        public string[] FilterItemValues { get; set; } = new string [] { };
        public string Game { get; set; } = "dota2";
        public string GameName { get; set; }
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