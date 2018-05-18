using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class SearchOffersInfoViewModel
    {
        public string Game { get; set; } = "dota2";
        public IEnumerable<GameViewModel> Games { get; set; }
        public string UserId { get; set; }
        public string SearchString { get; set; } = "";
        public int Page { get; set; } = 1;

        public string Sort { get; set; } = "bestSeller";
        public string[] Filters { get; set; }
        public bool IsOnline { get; set; }
        public bool SearchInDiscription { get; set; }
        public IList<JsonFilter> JsonFilters { get; set; } = new List<JsonFilter>();
        public string[] FilterValues { get; set; }
        public string[] FilterItemValues { get; set; } = new string[] { };

        public decimal MinGamePrice { get; set; }

        public decimal MaxGamePrice { get; set; }

        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
    }
}