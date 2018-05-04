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
        public int Rank { get; set; } // порядковый номер среди рангов

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        public virtual Filter Filter { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
