using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Service
{
    public interface IFeedbackService
    {
        IEnumerable<Feedback> GetFeedbacks();
        int PositiveFeedbackCount(UserProfile user);
        int NegativeFeedbackCount(UserProfile user);
        void LeaveAutomaticFeedback(string sellerId, string buyerId, int orderId);
        double PositiveFeedbackProcent(int positiveFeedbacks, int negativeFeedbacks);
        double NegativeFeedbackProcent(int positiveFeedbacks, int negativeFeedbacks);
        Feedback GetFeedback(int id);
        
        void CreateFeedback(Feedback feedback);
        void SaveFeedback();
        Task SaveFeedbackAsync();
    }

    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository feedbacksRepository;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public FeedbackService(IFeedbackRepository feedbacksRepository, IUserProfileRepository userProfileRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.userProfileRepository = userProfileRepository;
            this.orderRepository = orderRepository;
            this.feedbacksRepository = feedbacksRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IMessageService Members

        public IEnumerable<Feedback> GetFeedbacks()
        {
            var feedback = feedbacksRepository.GetAll();
            return feedback;
        }

        public void LeaveAutomaticFeedback(string sellerId, string buyerId, int orderId)
        {
            var seller = userProfileRepository.GetUserById(sellerId);
            var buyer = userProfileRepository.GetUserById(buyerId);
            var order = orderRepository.GetById(orderId);
            if (seller != null && buyer != null && order != null)
            {
                if (order.BuyerId == buyerId && order.SellerId == sellerId) // добавить условие на статус заказа
                {
                    var feedbackToSeller = new Feedback()
                    {
                        Grade = Emotions.Good,
                        Comment = "Автоматический отзыв",
                        DateLeft = DateTime.Now,
                        Order = order,
                        UserFrom = buyer,
                        UserTo = seller
                    };
                    var feedbackToBuyer = new Feedback()
                    {
                        Grade = Emotions.Good,
                        Comment = "Автоматический отзыв",
                        DateLeft = DateTime.Now,
                        Order = order,
                        UserFrom = seller,
                        UserTo = buyer
                    };
                    if (!order.SellerFeedbacked)
                    {
                        seller.FeedbacksToOthers.Add(feedbackToBuyer);
                        order.SellerFeedbacked = true;
                        
                    }
                    if (!order.BuyerFeedbacked)
                    {
                        buyer.FeedbacksToOthers.Add(feedbackToSeller);
                        order.BuyerFeedbacked = true;
                    }
                }
            }
        }

        public int PositiveFeedbackCount(UserProfile user)
        {
            int positiveCount = user.FeedbacksMy.Where(f => f.Grade == Emotions.Good).Count();
            return positiveCount;
        }

        public int NegativeFeedbackCount(UserProfile user)
        {
            int negativeCount = user.FeedbacksMy.Where(f => f.Grade == Emotions.Bad).Count();
            return negativeCount;
        }

        public double PositiveFeedbackProcent(int positiveFeedbacks, int negativeFeedbacks)
        {
            int allFeedbackCount = positiveFeedbacks + negativeFeedbacks;

            double pos = Math.Round((double)(100 * positiveFeedbacks) / (allFeedbackCount), 2);
            return pos;
        }

        public double NegativeFeedbackProcent(int positiveFeedbacks, int negativeFeedbacks)
        {
            int allFeedbackCount = positiveFeedbacks + negativeFeedbacks;
            
            double neg = Math.Round((double)(100 * negativeFeedbacks) / (allFeedbackCount) , 2);
            return neg;
        }

        public Feedback GetFeedback(int id)
        {
            var feedback = feedbacksRepository.GetById(id);
            return feedback;
        }


        public void CreateFeedback(Feedback feedback)
        {
            feedbacksRepository.Add(feedback);
        }

        public void SaveFeedback()
        {
            unitOfWork.Commit();
        }

        public async Task SaveFeedbackAsync()
        {
            await unitOfWork.CommitAsync();
        }

        #endregion

    }
}
