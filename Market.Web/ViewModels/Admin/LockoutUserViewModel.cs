﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class LockoutUserViewModel
    {
        public string UserId { get; set; }
        public string LockoutReason { get; set; }
    }
}