using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class FilterItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Image { get; set; }

        public virtual Filter Filter { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
