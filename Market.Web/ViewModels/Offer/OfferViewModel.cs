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
        public string Game { get; set; }        
        public decimal Price { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public UserProfile User { get; set; }
        public ICollection<Filter> Filters { get; set; }

    }
}