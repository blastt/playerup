using Market.Model.Models;
using Market.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels.Offer
{
    public class OfferListViewModel
    {
        public Game Game { get; set; }
        public ICollection<CreateFilterViewModel> Filters { get; set; }
        public ICollection<OfferViewModel> Offers { get; set; }
    }
}