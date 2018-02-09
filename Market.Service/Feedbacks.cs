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
