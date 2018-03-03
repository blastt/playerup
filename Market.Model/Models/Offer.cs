using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Model.Models
{


    public class Offer
    {
        public int Id { get; set; }
        public string Header { get; set; }
       
        public string Discription { get; set; }
        public string SteamLogin { get; set; }
        
        public decimal Price { get; set; }
        public int Views { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateDeleted { get; set; }



        public virtual Game Game { get; set; }
        public virtual Order Order { get; set; }
        public virtual string UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual IList<Filter> Filters { get; set; } = new List<Filter>();
        public virtual IList<FilterItem> FilterItems { get; set; } = new List<FilterItem>();
    }
}
