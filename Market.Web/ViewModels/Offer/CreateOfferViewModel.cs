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
        public SelectList Games { get; set; }
        public SelectList Filters { get; set; }
        public SelectList FilterItems { get; set; }
        public decimal Price { get; set; }
    }
}