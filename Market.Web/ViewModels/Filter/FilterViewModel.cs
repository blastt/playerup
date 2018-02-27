using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class FilterViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ICollection<FilterItemViewModel> FilterItems { get; set; }
        public string GameValue { get; set; }
    }
}