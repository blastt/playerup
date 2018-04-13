using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class Dialog
    {
        public int Id { get; set; }

        public virtual string CreatorId { get; set; }
        public virtual UserProfile Creator { get; set; }

        public virtual string CompanionId { get; set; }
        public virtual UserProfile Companion { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
