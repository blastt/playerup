using Market.Model.Models;
using System.Collections.Generic;

namespace Market.Web.ViewModels
{
    public class DialogViewModel
    {
        public int Id { get; set; }

        

        public string otherUserId { get; set; }
        public string otherUserName { get; set; }
        public string otherUserImage { get; set; }

        public int CountOfNewMessages { get; set; }

        public string CreatorId { get; set; }
        public UserProfile Creator { get; set; }

        public string CompanionId { get; set; }
        public UserProfile Companion { get; set; }
        public ICollection<MessageViewModel> Messages { get; set; }
    }
}