using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class Filter
    {
        public int Id { get; set; }
        public string Value { get; set; }        
        public string Text { get; set; }
        public virtual Game Game { get; set; }

        public virtual ICollection<FilterItem> FilterItems { get; set; }
        
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
