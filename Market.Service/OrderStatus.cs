using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Service
{
    public interface IOrderStatusService
    {
        IEnumerable<OrderStatus> GetOrderStatuses();
        OrderStatus GetOrderStatus(int id);
        OrderStatus GetOrderStatusByValue(string name);
        OrderStatus GetCurrentOrderStatus(Order order);
        void CreateOrderStatus(OrderStatus message);
        void SaveOrderStatus();
    }

    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderStatusRepository orderStatusesRepository;
        private readonly IOrderRepository ordersRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderStatusService(IOrderStatusRepository orderStatusesRepository, IOrderRepository ordersRepository, IUnitOfWork unitOfWork)
        {
            this.orderStatusesRepository = orderStatusesRepository;
            this.unitOfWork = unitOfWork;
            this.ordersRepository = ordersRepository;
        }

        #region IOrderStatusService Members

        public IEnumerable<OrderStatus> GetOrderStatuses()
        {
            var orderStatuss = orderStatusesRepository.GetAll();
            return orderStatuss;
        }


        public OrderStatus GetOrderStatus(int id)
        {
            var orderStatus = orderStatusesRepository.GetById(id);
            return orderStatus;
        }

        public OrderStatus GetCurrentOrderStatus(Order order)
        {
            if (order != null)
            {
                var currentOrderStatus = order.OrderStatuses.OrderBy(o => o.DateFinished).LastOrDefault();
                return currentOrderStatus;
            }
            return null;
            
        }


        public void CreateOrderStatus(OrderStatus orderStatus)
        {
            orderStatusesRepository.Add(orderStatus);
        }

        public void SaveOrderStatus()
        {
            unitOfWork.Commit();
        }

        public OrderStatus GetOrderStatusByValue(string name)
        {
            return orderStatusesRepository.GetOrderStatusByValue(name);
        }

        #endregion

    }
}

