using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System.Collections.Generic;

namespace Market.Service
{
    public interface IOrderStatusService
    {
        IEnumerable<OrderStatus> GetOrderStatuses();
        OrderStatus GetOrderStatus(int id);
        OrderStatus GetOrderStatusByValue(OrderStatuses value);
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

        public OrderStatus GetOrderStatusByValue(OrderStatuses value)
        {

            var orderStatus = orderStatusesRepository.GetOrderStatusByValue(value);
            return orderStatus;
        }


        public void CreateOrderStatus(OrderStatus orderStatus)
        {
            orderStatusesRepository.Add(orderStatus);
        }

        public void SaveOrderStatus()
        {
            unitOfWork.Commit();
        }


        #endregion

    }
}

