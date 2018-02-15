using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class Filter
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public virtual Game Game { get; set; }

        public virtual IEnumerable<FilterItem> FilterItems { get; set; }
        public virtual IEnumerable<Offer> Offers { get; set; }
    }
}
