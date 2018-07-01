using System.Collections.Generic;

namespace Market.Model.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public string ImagePath { get; set; }

        public int Rank { get; set; } // порядковый номер среди рангов

        public ICollection<Offer> Offers { get; set; }
        public ICollection<Filter> Filters { get; set; }
    }
}
