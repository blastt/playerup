using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class SearchFeedbacksInfoViewModel
    {
        public string UserId { get; set; }
        public int Page { get; set; } = 1;
    }
}