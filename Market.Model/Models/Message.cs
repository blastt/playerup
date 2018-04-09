using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Model.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool FromViewed { get; set; }
        public bool ToViewed { get; set; }
        public int ParentMessageId { get; set; }
        
        public bool SenderDeleted { get; set; }
        public bool ReceiverDeleted { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual string SenderId { get; set; }
        public virtual string ReceiverId { get; set; }
        public virtual UserProfile Sender { get; set; }
        public virtual UserProfile Receiver { get; set; }

        public virtual int DialogId { get; set; }
        public virtual Dialog Dialog { get; set; }
        
    }
}
