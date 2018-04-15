
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
        public string Id { get; set; }
        public byte[] Avatar { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        //public decimal Balance { get; set; }
        public string Discription { get; set; }
        public bool IsOnline { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string Name { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Message> MessagesAsSender { get; set; } = new List<Message>();
        public virtual ICollection<Message> MessagesAsReceiver { get; set; } = new List<Message>();
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

        public virtual ICollection<Order> OrdersAsSeller { get; set; } = new List<Order>();
        public virtual ICollection<Order> OrdersAsBuyer { get; set; } = new List<Order>();
        public virtual ICollection<Order> OrdersAsMiddleman { get; set; } = new List<Order>();

        public virtual ICollection<Feedback> FeedbacksMy { get; set; } = new List<Feedback>();
        public virtual ICollection<Feedback> FeedbacksToOthers { get; set; } = new List<Feedback>();

        public virtual ICollection<Dialog> DialogsAsCreator { get; set; } = new List<Dialog>();
        public virtual ICollection<Dialog> DialogsAsСompanion { get; set; } = new List<Dialog>();
    }
}
