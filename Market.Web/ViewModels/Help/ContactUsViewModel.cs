using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class ContactUsViewModel
    {
        public string Email { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
    }
}