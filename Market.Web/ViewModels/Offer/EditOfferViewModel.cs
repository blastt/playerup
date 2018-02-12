using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class EditOfferViewModel
    {
        public string Header { get; set; }
        public string Game { get; set; }
        public string Discription { get; set; }
        public string SteamLogin { get; set; }
        public string Filter { get; set; }
        public decimal Price { get; set; }
    }
}