﻿using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class DetailsFilterItemViewModel
    {
        public IEnumerable<Game> Games { get; set; }
    }
}