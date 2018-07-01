using Market.Data.Infrastructure;
using Market.Model.Models;

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
