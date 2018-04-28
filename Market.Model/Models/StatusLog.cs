using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Model.Models
{
    public class StatusLog
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; } // Время изменения статуса
        public virtual int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public virtual int OldStatusId { get; set; }
        public virtual OrderStatus OldStatus { get; set; } // с какого

        public virtual int NewStatusId { get; set; }
        public virtual OrderStatus NewStatus { get; set; } // на какой
       
    }
}
