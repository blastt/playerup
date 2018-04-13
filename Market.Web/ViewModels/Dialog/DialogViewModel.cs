using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace Market.Web.ViewModels
{
    public class DialogViewModel
    {
        public int Id { get; set; }

        

        public string otherUserId { get; set; }
        public string otherUserName { get; set; }

        public int CountOfNewMessages { get; set; }

        public string CreatorId { get; set; }
        public UserProfile Creator { get; set; }

        public string CompanionId { get; set; }
        public UserProfile Companion { get; set; }
        public ICollection<MessageViewModel> Messages { get; set; }
    }
}