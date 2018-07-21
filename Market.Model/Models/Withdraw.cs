using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class Withdraw
    {
        public int Id { get; set; }
        public string PaywayName { get; set; }
        public string Details { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime DateCrated { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public UserProfile User { get; set; }
    }
}
