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
    enum Closer
    {
        Buyer,
        Seller,
        Middleman,
        Automatically
    }

    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();
        //IEnumerable<Offer> GetCategoryGadgets(string categoryName, string gadgetName = null);
        Order GetOrder(int id);
        Order GetOrder(string accountLogin, string moderatorId, string sellerId, string buyerId);
        void UpdateOrder(Order order);
        void CreateOrder(Order order);
        bool CloseOrderByBuyer(int orderId);
        bool CloseOrderBySeller(int orderId);
        bool CloseOrderByMiddleman(int orderId);
        bool CloseOrderAutomatically(int orderId);
        bool ConfirmOrder(int orderId, string currentUserId);
        void SaveOrder();
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository ordersRepository;
        private readonly IFeedbackRepository feedbacksRepository;
        private readonly IOrderStatusRepository orderStatusRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly ITransactionRepository transactionRepository;

        public OrderService(IOrderRepository ordersRepository, IFeedbackRepository feedbacksRepository, ITransactionRepository transactionRepository, IOrderStatusRepository orderStatusRepository, IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            this.feedbacksRepository = feedbacksRepository;
            this.orderStatusRepository = orderStatusRepository;
            this.ordersRepository = ordersRepository;
            this.unitOfWork = unitOfWork;
            this.userProfileRepository = userProfileRepository;
            this.transactionRepository = transactionRepository;
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

        private bool CloseOrder(int orderId, Closer closer)
        {
            var order = GetOrder(orderId);
            if (order != null)
            {
                if (order.CurrentStatus.Value == OrderStatuses.BuyerPaying ||
                    order.CurrentStatus.Value == OrderStatuses.OrderCreating ||
                    order.CurrentStatus.Value == OrderStatuses.MiddlemanFinding ||
                    order.CurrentStatus.Value == OrderStatuses.SellerProviding ||
                    order.CurrentStatus.Value == OrderStatuses.MidddlemanChecking)
                {
                    OrderStatus newOrderStatus = null;

                    switch (closer)
                    {
                        case Closer.Buyer:
                            {
                                newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.BuyerClosed);
                            }
                            break;
                        case Closer.Seller:
                            {
                                newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.SellerClosed);
                            }
                            break;
                        case Closer.Middleman:
                            {
                                newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.MiddlemanClosed);
                            }
                            break;
                        case Closer.Automatically:
                            {
                                newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.ClosedAutomatically);
                            }
                            break;
                        default:
                            {
                                newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.ClosedAutomatically);
                            }
                            break;
                    }



                    if (newOrderStatus != null)
                    {
                        var orderTransactions = order.Transactions.ToList();

                        foreach (var transaction in orderTransactions)
                        {
                            transaction.Sender.Balance += transaction.Amount;
                            transaction.Receiver.Balance -= transaction.Amount;

                            order.Transactions.Add(new Transaction
                            {
                                Receiver = transaction.Sender,
                                Sender = transaction.Receiver,
                                Amount = transaction.Amount,
                                TransactionDate = DateTime.Now
                            });
                        }

                        order.StatusLogs.AddLast(new StatusLog()
                        {
                            OldStatus = order.CurrentStatus,
                            NewStatus = newOrderStatus,
                            TimeStamp = DateTime.Now
                        });
                        order.CurrentStatus = newOrderStatus;
                        order.BuyerChecked = false;
                        order.SellerChecked = false;
                        return true;
                    }

                }
                
            }
            return false;
        }

        public bool CloseOrderByBuyer(int orderId)
        {
            return CloseOrder(orderId, Closer.Buyer);
        }

        public bool CloseOrderBySeller(int orderId)
        {
            return CloseOrder(orderId, Closer.Buyer);
        }

       
        public bool CloseOrderByMiddleman(int orderId)
        {
            return CloseOrder(orderId, Closer.Buyer);
        }

        public bool CloseOrderAutomatically(int orderId)
        {
            return CloseOrder(orderId, Closer.Automatically);
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
                            decimal amount = order.AmmountSellerGet.Value;
                            transactionRepository.Add(new Transaction
                            {
                                Amount = amount,
                                Receiver = order.Seller,
                                Sender = mainCup,
                                Order = order,
                                TransactionDate = DateTime.Now
                            });

                            mainCup.Balance -= amount;                                                                                  
                            order.Seller.Balance += amount;
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
