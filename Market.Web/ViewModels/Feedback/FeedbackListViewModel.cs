using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class FeedbackListViewModel
    {

        public SearchViewModel SearchInfo { get; set; }
        public PageInfoViewModel PageInfo { get; set; }

        public IList<FeedbackViewModel> Feedbacks { get; set; }
    }
}