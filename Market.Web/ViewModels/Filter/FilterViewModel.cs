using System.Collections.Generic;

namespace Market.Web.ViewModels
{
    public class FilterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ICollection<FilterItemViewModel> FilterItems { get; set; }
        public string GameValue { get; set; }
        public string GameName { get; set; }
    }
}