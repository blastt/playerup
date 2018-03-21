﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class AccountInfo
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string AdditionalInformation { get; set; }

        public virtual Order Order { get; set; }

        public virtual UserProfile Buyer { get; set; }
        public virtual string BuyerId { get; set; }

        public virtual UserProfile Moderator { get; set; }
        public virtual string ModeratorId { get; set; }
    }
}