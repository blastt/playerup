using System.Collections.Generic;

namespace Market.Web.ViewModels
{
    public class MessageListViewModel
    {

        public ICollection<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();

    }
}