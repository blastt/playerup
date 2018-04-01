using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Repositories
{
    public class OrderStatusRepository : RepositoryBase<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public OrderStatus GetOrderStatusByValue(string name)
        {
            return DbContext.OrderStatuses.FirstOrDefault(g => g.Value == name);
        }
    }

    public interface IOrderStatusRepository : IRepository<OrderStatus>
    {
        OrderStatus GetOrderStatusByValue(string userName);
    }
}
