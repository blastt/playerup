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
        public Game Game { get; set; }
        public IEnumerable<SelectListItem> Games { get; set; }
        public IEnumerable<SelectListItem> Filters { get; set; }
        public IEnumerable<SelectListItem> FilterItems { get; set; }
        public decimal Price { get; set; }
    }
}