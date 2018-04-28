using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Repositories
{
    public class StatusLogRepository : RepositoryBase<StatusLog>, IStatusLogRepository
    {
        public StatusLogRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        
    }

    public interface IStatusLogRepository : IRepository<StatusLog>
    {
        
    }
}
