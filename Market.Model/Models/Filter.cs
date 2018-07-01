using System.Collections.Generic;

namespace Market.Model.Models
{
    public class Filter
    {
        public int Id { get; set; }
        public string Value { get; set; }        
        public string Text { get; set; }
        public Game Game { get; set; }

        public ICollection<FilterItem> FilterItems { get; set; }
               
        public ICollection<Offer> Offers { get; set; }
    }
}
