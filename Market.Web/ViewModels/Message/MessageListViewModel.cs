using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class MessageListViewModel
    {

        public ICollection<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();
    }
}