using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Model.Models
{
    public enum Emotions
    {
        Bad,
        Good
    }

    public class Feedback
    {
        public int Id { get; set; }
        public Emotions Grade { get; set; }
        public string Comment { get; set; }
        public DateTime? DateLeft { get; set; }
   
        public string OfferId { get; set; }        

        public string OfferHeader { get; set; }

        public virtual string SenderId { get; set; }
        public virtual string ReceiverId { get; set; }
        public virtual UserProfile Sender { get; set; }
        public virtual UserProfile Receiver { get; set; }

    }
}
