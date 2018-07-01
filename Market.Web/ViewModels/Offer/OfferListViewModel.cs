using Market.Model.Models;
using System.Collections.Generic;

namespace Market.Web.ViewModels
{
    public class OfferListViewModel
    {
        public Game Game { get; set; }
        
        public IEnumerable<GameViewModel> Games { get; set; }
        public IEnumerable<GameViewModel> GamesForSelect { get; set; }
        public SearchViewModel SearchInfo { get; set; }
        public PageInfoViewModel PageInfo { get; set; }


        public int ActiveOffersCount { get; set; }
        public int InactiveOffersCount { get; set; }
        public int CloseOffersCount { get; set; }

        public IEnumerable<Model.Models.Filter> Filters { get; set; }
        public IEnumerable<OfferViewModel> Offers { get; set; }
    }
}