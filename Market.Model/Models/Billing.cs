using System;

namespace Market.Model.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCeated { get; set; }

        public string UserId { get; set; }
        public UserProfile User { get; set; }
    }
}
