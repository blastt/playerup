using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Discription { get; set; }
        public Game Game { get; set; }        
        public decimal Price { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public UserProfile User { get; set; }
        
        public IList<Filter> Filters { get; set; }
        public IList<FilterItem> FilterItems { get; set; }
    }
}