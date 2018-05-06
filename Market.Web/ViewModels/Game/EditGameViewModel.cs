using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class EditGameViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public int Rank { get; set; }

        public string ImagePath { get; set; }
    }
}