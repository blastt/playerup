using Market.Model.Models;
using Market.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.ViewModels
{
    public class OfferListViewModel
    {
        public Game Game { get; set; }
        
        public IEnumerable<GameViewModel> Games { get; set; }
        public IEnumerable<GameViewModel> GamesForSelect { get; set; }
        public SearchViewModel SearchInfo { get; set; }
        public PageInfoViewModel PageInfo { get; set; }
        public IEnumerable<Model.Models.Filter> Filters { get; set; }
        public IEnumerable<OfferViewModel> Offers { get; set; }
    }
}