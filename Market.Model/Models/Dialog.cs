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

        public virtual ICollection<UserProfile> Users { get; set; } = new List<UserProfile>();

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
