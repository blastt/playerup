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

        public ICollection<UserProfileViewModel> Users { get; set; }
        public ICollection<MessageViewModel> Messages { get; set; }
    }
}