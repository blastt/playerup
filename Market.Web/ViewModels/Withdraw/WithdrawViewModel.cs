using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class WithdrawViewModel
    {
        public int Id { get; set; }

        public string PaywayName { get; set; }

        public string Details { get; set; }

        public decimal Amount { get; set; }
        public DateTime DateCrated { get; set; } = DateTime.Now;
        public string UserName { get; set; }

        public bool IsPaid { get; set; }
        public bool IsCanceled { get; set; }

    }
}