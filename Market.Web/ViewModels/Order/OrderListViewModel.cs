using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class OrderListViewModel
    {
        public SearchViewModel SearchInfo { get; set; }
        public PageInfoViewModel PageInfo { get; set; }
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}