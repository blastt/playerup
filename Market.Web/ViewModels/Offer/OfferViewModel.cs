using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal Price { get; set; }
        public bool SellerPaysMiddleman { get; set; }
        public decimal MiddlemanPrice { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public UserProfile User { get; set; }
        
        public IList<Filter> Filters { get; set; }
        public IList<FilterItem> FilterItems { get; set; }


        public Dictionary<Model.Models.Filter, FilterItem> FilterFilterItem { get; set; }
    }
}