using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class ScreenshotPath
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public virtual int OfferId { get; set; }
        public virtual Offer Offer { get; set; }
    }
}
