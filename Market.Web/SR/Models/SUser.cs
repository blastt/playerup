using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.SR.Models
{
    public class SUser
    {
        public SUser()
        {
            ConnectionId = new List<string>();
        }
        public string Id { get; set; }
        public List<string> ConnectionId { get; set; }
    }
}