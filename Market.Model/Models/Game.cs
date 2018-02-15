using System;
using System.Collections.Generic;
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

        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<Filter> Filters { get; set; }
    }
}
