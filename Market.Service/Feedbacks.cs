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
    public interface IFeedbackService
    {
        IEnumerable<Feedback> GetFeedbacks();
        int PositiveFeedbackCount(UserProfile user);
        int NegativeFeedbackCount(UserProfile user);
        double PositiveFeedbackProcent(int positiveFeedbacks, int negativeFeedbacks);
        double NegativeFeedbackProcent(int positiveFeedbacks, int negativeFeedbacks);
        Feedback GetFeedback(int id);
        
        void CreateFeedback(Feedback feedback);
        void SaveFeedback();
    }

    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository feedbacksRepository;
        private readonly IUnitOfWork unitOfWork;

        public FeedbackService(IFeedbackRepository feedbacksRepository, IUnitOfWork unitOfWork)
        {
            this.feedbacksRepository = feedbacksRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IMessageService Members

        public IEnumerable<Feedback> GetFeedbacks()
        {
            var feedback = feedbacksRepository.GetAll();
            return feedback;
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

        #endregion

    }
}
