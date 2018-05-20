using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class GiveFeedbackViewModel
    {

        public Emotions Grade { get; set; }

        [Display(Name = "Отзыв")]
        public string Comment { get; set; }
        public int? OrderId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }

    }
}