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
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();
        //IEnumerable<Offer> GetCategoryGadgets(string categoryName, string gadgetName = null);
        Order GetOrder(int id);
        void CreateOrder(Order order);
        void SaveOrder();
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository ordersRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IOrderRepository ordersRepository, IUnitOfWork unitOfWork)
        {
            this.ordersRepository = ordersRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IOrderService Members

        public IEnumerable<Order> GetOrders()
        {
            var orders = ordersRepository.GetAll();
            return orders;
        }


        public Order GetOrder(int id)
        {
            var order = ordersRepository.GetById(id);
            return order;
        }

        public void CreateOrder(Order order)
        {
            ordersRepository.Add(order);
        }

        public void SaveOrder()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}
