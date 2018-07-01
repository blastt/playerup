using System.Collections.Generic;

namespace Market.Web.ViewModels
{
    public class FeedbackListViewModel
    {

        public SearchViewModel SearchInfo { get; set; }
        public PageInfoViewModel PageInfo { get; set; }

        public IEnumerable<FeedbackViewModel> Feedbacks { get; set; }
    }
}