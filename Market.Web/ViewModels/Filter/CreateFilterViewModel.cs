using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.ViewModels
{
    public class CreateFilterViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Game Game { get; set; }

        public IList<SelectListItem> Games { get; set; }
    }
}