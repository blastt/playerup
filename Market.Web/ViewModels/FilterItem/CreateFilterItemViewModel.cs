using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.ViewModels
{
    public class CreateFilterItemViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        
        public bool CreateForExistsItem { get; set; }

        public Game Game { get; set; }
        public Model.Models.Filter Filter { get; set; }

        public IList<SelectListItem> Filters { get; set; }

        public IList<SelectListItem> Games { get; set; }
    }
}