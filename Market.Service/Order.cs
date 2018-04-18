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
        Order GetOrder(string accountLogin, string moderatorId, string sellerId, string buyerId);
        void UpdateOrder(Order order);
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

        public void UpdateOrder(Order order)
        {
            ordersRepository.Update(order);
        }
        public Order GetOrder(int id)
        {
            var order = ordersRepository.GetById(id);
            return order;
        }

        public Order GetOrder(string accountLogin, string middlemanId, string sellerId, string buyerId)
        {            
            Order order = ordersRepository.GetMany(o => o.Offer.AccountLogin == accountLogin && 
            o.MiddlemanId == middlemanId && o.BuyerId == buyerId && o.SellerId == sellerId).FirstOrDefault();            
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
