using System.Collections.Generic;

namespace Market.Web.ViewModels
{
    public class DialogListViewModel
    {


        public SearchViewModel SearchInfo { get; set; }
        public PageInfoViewModel PageInfo { get; set; }

        public int DialogsCount { get; set; }

        public IEnumerable<DialogViewModel> Dialogs { get; set; }

    }
}