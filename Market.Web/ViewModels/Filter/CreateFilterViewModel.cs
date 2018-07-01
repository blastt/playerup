using System.Collections.Generic;
using System.Web.Mvc;

namespace Market.Web.ViewModels
{
    public class CreateFilterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Game { get; set; }

        public IList<SelectListItem> Games { get; set; } = new List<SelectListItem>();
    }
}