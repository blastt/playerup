using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class SearchOffersInfoViewModel
    {
        public string Game { get; set; } = "dota2";
        public string UserId { get; set; }
        public string SearchString { get; set; } = "";
        public int Page { get; set; } = 1;
    }
}