using Market.Model.Models;
using System;

namespace Market.Web.ViewModels
{
    public class FeedbackViewModel
    {
        public Emotions Grade { get; set; }
        public string Comment { get; set; }
        public DateTime? DateLeft { get; set; }
        public string SenderName { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string OfferHeader { get; set; }
    }
}