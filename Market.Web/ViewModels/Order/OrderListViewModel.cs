using System.Collections.Generic;

namespace Market.Web.ViewModels
{
    public class OrderListViewModel
    {
        public SearchViewModel SearchInfo { get; set; }
        public PageInfoViewModel PageInfo { get; set; }
        public IEnumerable<OrderViewModel> Orders { get; set; }

        public int BuyCount { get; set; }
        public int SellCount { get; set; }
    }
}