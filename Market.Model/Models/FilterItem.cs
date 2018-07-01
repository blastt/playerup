using System.Collections.Generic;

namespace Market.Model.Models
{
    public class FilterItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Rank { get; set; } // порядковый номер среди рангов

        public string ImagePath { get; set; }

        public Filter Filter { get; set; }
        public ICollection<Offer> Offers { get; set; }
    }
}
