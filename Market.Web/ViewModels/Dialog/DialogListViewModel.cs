using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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