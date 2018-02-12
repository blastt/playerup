using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Model.Models
{
    public class Message
    {

        public int Id { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool IsViewed { get; set; }
        public int ParentMessageId { get; set; }
        //public string SenderId { get; set; }
        //public string ReceiverId { get; set; }
        public bool SenderDeleted { get; set; }
        public bool ReceiverDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual IList<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
    }
}
