using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Web.ViewModels
{
    public class OfferViewModel
    {
        
        public int Id { get; set; }
        public string Header { get; set; }
        public string Discription { get; set; }
        public Game Game { get; set; }
        public bool HasOrder { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        public bool SellerPaysMiddleman { get; set; }

        [DataType(DataType.Currency)]
        public decimal MiddlemanPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateDeleted { get; set; }
        public UserProfile User { get; set; }

        public string UserName { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }


        public IList<Filter> Filters { get; set; }
        public IList<FilterItem> FilterItems { get; set; }


        public Dictionary<Model.Models.Filter, FilterItem> FilterFilterItem { get; set; }
    }
}