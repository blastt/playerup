using System;


namespace Market.Model.Models
{
    public class Message
    {
        public int Id { get; set; }
        //public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool FromViewed { get; set; }
        public bool ToViewed { get; set; }
        //public int ParentMessageId { get; set; }
        
        public bool SenderDeleted { get; set; }
        public bool ReceiverDeleted { get; set; }
        public DateTime CreatedDate { get; set; }

        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public UserProfile Sender { get; set; }
        public UserProfile Receiver { get; set; }

        public int DialogId { get; set; }
        public Dialog Dialog { get; set; }
        
    }
}
