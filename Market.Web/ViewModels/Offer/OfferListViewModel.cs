using Market.Model.Models;
using Market.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class OfferListViewModel
    {
        public Game Game { get; set; }
        
        public IEnumerable<Game> Games { get; set; }
        public SearchViewModel SearchInfo { get; set; }
        public IEnumerable<Filter> Filters { get; set; }
        public ICollection<OfferViewModel> Offers { get; set; }
    }
}