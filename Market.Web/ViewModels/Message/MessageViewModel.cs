using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }

        [Required]
        public string MessageBody { get; set; } = "";
        public bool FromViewed { get; set; }
        public bool ToViewed { get; set; }
        //public string SenderId { get; set; }
        //public string ReceiverId { get; set; }
        public string SenderName { get; set; }
        public string SenderId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverId { get; set; }

        public bool IsCurrentUserMessage { get; set; }

        public string OfferHeader { get; set; }

        public bool SenderDeleted { get; set; }
        public bool ReceiverDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}