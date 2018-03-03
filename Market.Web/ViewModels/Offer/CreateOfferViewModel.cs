using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.ViewModels
{
    public class CreateOfferViewModel
    {
        public string Header { get; set; }
        public string Discription { get; set; }
        public string SteamLogin { get; set; }
        public string Game { get; set; }
        public string[] FilterValues { get; set; }
        public string[] FilterItemValues { get; set; }
        public IEnumerable<SelectListItem> Games { get; set; }
        public IEnumerable<SelectListItem> Filters { get; set; }
        public IEnumerable<SelectListItem> FilterItems { get; set; }
        public Dictionary<Model.Models.Filter, FilterItem> FilterPairs { get; set; }
        public decimal Price { get; set; }

        public CreateOfferViewModel()
        {
            Games = new List<SelectListItem>();
            Filters = new List<SelectListItem>();
            FilterItems = new List<SelectListItem>();

        }
    }
}