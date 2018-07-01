using System.Collections.Generic;

namespace Market.Model.Models
{
    public class Dialog
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }
        public UserProfile Creator { get; set; }

        public string CompanionId { get; set; }
        public UserProfile Companion { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
