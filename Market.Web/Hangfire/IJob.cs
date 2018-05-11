using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Web.Hangfire
{
    interface IJob
    {
        
        void Do(int itemId);
    }
}
