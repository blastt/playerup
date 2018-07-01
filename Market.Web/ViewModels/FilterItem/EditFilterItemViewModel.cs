using System.Collections.Generic;
using System.Web.Mvc;

namespace Market.Web.ViewModels
{
    public class EditFilterItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string FilterValue { get; set; }
        public string GameName { get; set; }
        public int Rank { get; set; }

        public string ImagePath { get; set; }

        public IList<SelectListItem> Filters { get; set; } = new List<SelectListItem>();
    }
}