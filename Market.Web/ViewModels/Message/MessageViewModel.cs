﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class MessageViewModel
    {
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool IsViewed { get; set; }
        //public string SenderId { get; set; }
        //public string ReceiverId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public bool SenderDeleted { get; set; }
        public bool ReceiverDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}