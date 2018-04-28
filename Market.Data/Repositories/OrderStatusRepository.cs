﻿using Market.Data.Infrastructure;
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

        public OrderStatus GetOrderStatusByValue(OrderStatuses value)
        {
            return DbContext.OrderStatuses.FirstOrDefault(g => g.Value == value);
        }
    }

    public interface IOrderStatusRepository : IRepository<OrderStatus>
    {
        OrderStatus GetOrderStatusByValue(OrderStatuses value);
    }
}
