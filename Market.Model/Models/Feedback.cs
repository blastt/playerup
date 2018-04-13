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
        public DateTime? DateLeft { get; set; } = DateTime.Now;

        public virtual int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public virtual string BuyerId { get; set; }
        public virtual UserProfile Buyer { get; set; }

        public virtual string SellerId { get; set; }
        public virtual UserProfile Seller { get; set; }

    }
}
