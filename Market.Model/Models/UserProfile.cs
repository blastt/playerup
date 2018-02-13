
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Model.Models
{
    public class UserProfile
    {
        [Key]
        public string Id { get; set; }
        public byte[] Avatar { get; set; }
        public int Rating { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        public string Discription { get; set; }
        public int MyProperty { get; set; }
        public bool IsOnline { get; set; }

        public string Name { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
