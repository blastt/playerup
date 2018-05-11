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
        bool CloseOrderByBuyer(Order order);
        bool CloseOrderBySeller(Order order);
        bool CloseOrderByMiddleman(Order order);
        bool CloseOrderAutomatically(int orderId);
        bool ConfirmOrder(int orderId, string currentUserId);
        void SaveOrder();
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository ordersRepository;
        private readonly IOrderStatusRepository orderStatusRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly ISellerInvoiceRepository sellerInvoicerepository;

        public OrderService(IOrderRepository ordersRepository, ISellerInvoiceRepository sellerInvoicerepository, IOrderStatusRepository orderStatusRepository, IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            this.orderStatusRepository = orderStatusRepository;
            this.ordersRepository = ordersRepository;
            this.unitOfWork = unitOfWork;
            this.userProfileRepository = userProfileRepository;
            this.sellerInvoicerepository = sellerInvoicerepository;
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


        public bool CloseOrderByBuyer(Order order)
        {
            if (order.CurrentStatus.Value == OrderStatuses.BuyerPaying ||
                    order.CurrentStatus.Value == OrderStatuses.OrderCreating ||
                    order.CurrentStatus.Value == OrderStatuses.MiddlemanFinding ||
                    order.CurrentStatus.Value == OrderStatuses.SellerProviding ||
                    order.CurrentStatus.Value == OrderStatuses.MidddlemanChecking)
            {
                OrderStatus newOrderStatus = null;

                newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.BuyerClosed);

                if (newOrderStatus != null)
                {

                    order.StatusLogs.AddLast(new StatusLog()
                    {
                        OldStatus = order.CurrentStatus,
                        NewStatus = newOrderStatus,
                        TimeStamp = DateTime.Now
                    });
                    order.CurrentStatus = newOrderStatus;
      
                    return true;
                }

            }
            return false;
        }

        public bool CloseOrderBySeller(Order order)
        {
            if (order.CurrentStatus.Value == OrderStatuses.BuyerPaying ||
                    order.CurrentStatus.Value == OrderStatuses.OrderCreating ||
                    order.CurrentStatus.Value == OrderStatuses.MiddlemanFinding ||
                    order.CurrentStatus.Value == OrderStatuses.SellerProviding ||
                    order.CurrentStatus.Value == OrderStatuses.MidddlemanChecking)
            {
                OrderStatus newOrderStatus = null;

                newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.SellerClosed);

                if (newOrderStatus != null)
                {

                    order.StatusLogs.AddLast(new StatusLog()
                    {
                        OldStatus = order.CurrentStatus,
                        NewStatus = newOrderStatus,
                        TimeStamp = DateTime.Now
                    });
                    order.CurrentStatus = newOrderStatus;
            
                    return true;
                }

            }
            return false;
        }

       
        public bool CloseOrderByMiddleman(Order order)
        {
            if (order.CurrentStatus.Value == OrderStatuses.BuyerPaying ||
                    order.CurrentStatus.Value == OrderStatuses.OrderCreating ||
                    order.CurrentStatus.Value == OrderStatuses.MiddlemanFinding ||
                    order.CurrentStatus.Value == OrderStatuses.SellerProviding ||
                    order.CurrentStatus.Value == OrderStatuses.MidddlemanChecking)
            {
                OrderStatus newOrderStatus = null;

                newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.MiddlemanClosed);

                if (newOrderStatus != null)
                {

                    order.StatusLogs.AddLast(new StatusLog()
                    {
                        OldStatus = order.CurrentStatus,
                        NewStatus = newOrderStatus,
                        TimeStamp = DateTime.Now
                    });
                    order.CurrentStatus = newOrderStatus;
              
                    return true;
                }

            }
            return false;
        }

        public bool CloseOrderAutomatically(int orderId)
        {
            var order = GetOrder(orderId);
            if (order.CurrentStatus.Value == OrderStatuses.BuyerPaying ||
                    order.CurrentStatus.Value == OrderStatuses.OrderCreating ||
                    order.CurrentStatus.Value == OrderStatuses.MiddlemanFinding ||
                    order.CurrentStatus.Value == OrderStatuses.SellerProviding ||
                    order.CurrentStatus.Value == OrderStatuses.MidddlemanChecking)
            {
                OrderStatus newOrderStatus = null;

                newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.ClosedAutomatically);

                if (newOrderStatus != null)
                {

                    order.StatusLogs.AddLast(new StatusLog()
                    {
                        OldStatus = order.CurrentStatus,
                        NewStatus = newOrderStatus,
                        TimeStamp = DateTime.Now
                    });
                    order.CurrentStatus = newOrderStatus;
                 
                    return true;
                }

            }
            return false;
        }
       

        public bool ConfirmOrder(int orderId, string currentUserId)
        {
            var order = GetOrder(orderId);
            if (order != null)
            {
                if (order.CurrentStatus != null)
                {
                    if (order.BuyerId == currentUserId && order.CurrentStatus.Value == OrderStatuses.BuyerConfirming)
                    {
                        var mainCup = userProfileRepository.GetUserByName("palyerup");
                        if (mainCup != null)
                        {
                            mainCup.Balance -= order.AmmountSellerGet.Value;
                            sellerInvoicerepository.Add(new SellerInvoice
                            {
                                Amount = order.Sum,
                                DatePay = DateTime.Now,
                                UserId = order.SellerId,
                                OrderId = order.Id
                            });
                            order.Seller.Balance += order.AmmountSellerGet.Value;
                            order.StatusLogs.AddLast(new StatusLog()
                            {
                                OldStatus = order.CurrentStatus,
                                NewStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.PayingToSeller),
                                TimeStamp = DateTime.Now
                            });
                            order.CurrentStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.PayingToSeller);

                            order.StatusLogs.AddLast(new StatusLog()
                            {
                                OldStatus = order.CurrentStatus,
                                NewStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.ClosedSuccessfully),
                                TimeStamp = DateTime.Now
                            });
                            order.CurrentStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.ClosedSuccessfully);

                            order.BuyerChecked = false;
                            order.SellerChecked = false;
                            
                            //TempData["orderBuyStatus"] = "Спасибо за подтверждение сделки! Сделка успешно закрыта.";
                            //return RedirectToAction("BuyDetails", "Order", new { id = order.Id });
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion

    }
}
