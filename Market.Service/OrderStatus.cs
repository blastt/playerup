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
        void CreateOrderStatus(OrderStatus message);
        void SaveOrderStatus();
    }

    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderStatusRepository orderStatusesRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderStatusService(IOrderStatusRepository orderStatusesRepository, IUnitOfWork unitOfWork)
        {
            this.orderStatusesRepository = orderStatusesRepository;
            this.unitOfWork = unitOfWork;
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

