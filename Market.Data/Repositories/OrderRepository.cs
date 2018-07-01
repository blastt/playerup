using Market.Data.Infrastructure;
using Market.Model.Models;

namespace Market.Data.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IOrderRepository : IRepository<Order>
    {

    }
}
