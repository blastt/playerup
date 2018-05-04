using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public string ImagePath { get; set; }

        public int Rank { get; set; } // порядковый номер среди рангов

        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<Filter> Filters { get; set; }
    }
}
