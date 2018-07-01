using Market.Data.Infrastructure;
using Market.Model.Models;

namespace Market.Data.Repositories
{
    public class BillingRepository : RepositoryBase<Billing>, IBillingRepository
    {
        public BillingRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IBillingRepository : IRepository<Billing>
    {

    }
}
