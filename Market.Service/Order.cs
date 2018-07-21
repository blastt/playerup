using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        IQueryable<Order> GetOrders(params Expression<Func<Order, object>>[] includes);
        IQueryable<Order> GetOrders(Expression<Func<Order, bool>> where, params Expression<Func<Order, object>>[] includes);
        IQueryable<Order> GetOrdersAsNoTracking();
        IQueryable<Order> GetOrdersAsNoTracking(Expression<Func<Order, bool>> where, params Expression<Func<Order, object>>[] includes);


        //IEnumerable<Offer> GetCategoryGadgets(string categoryName, string gadgetName = null);
        Order GetOrder(int id);
        Task<Order> GetOrderAsync(int id);

        Order GetOrder(string accountLogin, string moderatorId, string sellerId, string buyerId);
        Order GetOrder(string accountLogin, string moderatorId, string sellerId, string buyerId, params Expression<Func<Order, object>>[] include);
        Order GetOrder(int id, params Expression<Func<Order, object>>[] includes);
        void UpdateOrder(Order order);
        void CreateOrder(Order order);
        bool ConfirmAbortOrder(int orderId, string userId);
        bool AbortOrder(int orderId, string currentUserId);
        bool ConfirmOrderByMiddleman(int orderId, string currentUserId);
        bool CloseOrderByBuyer(int orderId);
        bool CloseOrderBySeller(int orderId);
        bool CloseOrderByMiddleman(int orderId);
        bool CloseOrderAutomatically(int orderId);
        bool ConfirmOrder(int orderId, string currentUserId);
        void SaveOrder();
        Task SaveOrderAsync();
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

        public IQueryable<Order> GetOrders(params Expression<Func<Order, object>>[] includes)
        {
            var orders = ordersRepository.GetAll(includes);
            return orders;
        }

        public IQueryable<Order> GetOrders(Expression<Func<Order, bool>> where, params Expression<Func<Order, object>>[] includes)
        {
            var query = ordersRepository.GetMany(where, includes);
            return query;
        }
        public IQueryable<Order> GetOrdersAsNoTracking()
        {
            var query = ordersRepository.GetAllAsNoTracking();
            return query;
        }
        public IQueryable<Order> GetOrdersAsNoTracking(Expression<Func<Order, bool>> where, params Expression<Func<Order, object>>[] includes)
        {
            var query = ordersRepository.GetManyAsNoTracking(where, includes);
            return query;
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

        public Order GetOrder(int id, params Expression<Func<Order, object>>[] includes)
        {
            var order = ordersRepository.Get(o => o.Id == id, includes);
            return order;
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            var order = await ordersRepository.GetAsync(o => o.Id == id);
            return order;
        }

        public Order GetOrder(string accountLogin, string middlemanId, string sellerId, string buyerId)
        {
            Order order = ordersRepository.GetMany(o => o.Offer.AccountLogin == accountLogin &&
            o.MiddlemanId == middlemanId && o.BuyerId == buyerId && o.SellerId == sellerId).FirstOrDefault();
            return order;

        }

        public Order GetOrder(string accountLogin, string middlemanId, string sellerId, string buyerId, params Expression<Func<Order, object>>[] includes)
        {
            Order order = ordersRepository.GetMany(o => o.Offer.AccountLogin == accountLogin &&
            o.MiddlemanId == middlemanId && o.BuyerId == buyerId && o.SellerId == sellerId, includes).FirstOrDefault();
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

        public async Task SaveOrderAsync()
        {
            await unitOfWork.CommitAsync();
        }

        public bool ConfirmAbortOrder(int orderId, string userId)
        {
            var order = GetOrder(orderId, i => i.CurrentStatus, i => i.StatusLogs, i => i.Transactions, i => i.Middleman, id => id.Transactions.Select(m => m.Sender), id => id.Transactions.Select(m => m.Receiver));
            if (order != null)
            {
                OrderStatus newOrderStatus = null;
                if (order.MiddlemanId == userId && order.CurrentStatus.Value == OrderStatuses.MiddlemanBackingAccount)
                {
                    newOrderStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.MiddlemanClosed);
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

        private bool CloseOrder(int orderId, Closer closer)
        {
            var order = GetOrder(orderId, i => i.CurrentStatus, i => i.StatusLogs, i => i.Transactions, id => id.Transactions.Select(m => m.Sender), id => id.Transactions.Select(m => m.Receiver));
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
            return CloseOrder(orderId, Closer.Seller);
        }


        public bool CloseOrderByMiddleman(int orderId)
        {
            return CloseOrder(orderId, Closer.Middleman);
        }

        public bool CloseOrderAutomatically(int orderId)
        {
            return CloseOrder(orderId, Closer.Automatically);
        }


        public bool ConfirmOrder(int orderId, string currentUserId)
        {
            var order = GetOrder(orderId, i => i.Buyer, i => i.CurrentStatus, i => i.Seller, i => i.StatusLogs);
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

        public bool ConfirmOrderByMiddleman(int orderId, string currentUserId)
        {
            var order = GetOrder(orderId, i => i.Seller, i => i.CurrentStatus, i => i.Seller, i => i.StatusLogs);
            if (order != null)
            {
                if (order.CurrentStatus != null)
                {
                    if (order.MiddlemanId == currentUserId && order.CurrentStatus.Value == OrderStatuses.MiddlemanBackingAccount)
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

        public bool AbortOrder(int orderId, string currentUserId)
        {
            var order = GetOrder(orderId, i => i.Buyer, i => i.CurrentStatus, i => i.StatusLogs);
            if (order != null)
            {
                if (order.CurrentStatus != null)
                {
                    if (order.BuyerId == currentUserId && order.CurrentStatus.Value == OrderStatuses.BuyerConfirming)
                    {


                        order.StatusLogs.AddLast(new StatusLog()
                        {
                            OldStatus = order.CurrentStatus,
                            NewStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.AbortedByBuyer),
                            TimeStamp = DateTime.Now
                        });
                        order.CurrentStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.AbortedByBuyer);

                        order.StatusLogs.AddLast(new StatusLog()
                        {
                            OldStatus = order.CurrentStatus,
                            NewStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.MiddlemanBackingAccount),
                            TimeStamp = DateTime.Now
                        });
                        order.CurrentStatus = orderStatusRepository.GetOrderStatusByValue(OrderStatuses.MiddlemanBackingAccount);



                        order.BuyerChecked = false;
                        order.SellerChecked = false;



                        //TempData["orderBuyStatus"] = "Спасибо за подтверждение сделки! Сделка успешно закрыта.";
                        //return RedirectToAction("BuyDetails", "Order", new { id = order.Id });
                        return true;

                    }
                }
            }
            return false;
        }

        #endregion

    }
}
