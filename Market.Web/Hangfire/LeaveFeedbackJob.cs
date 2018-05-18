using Market.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Market.Web.Hangfire
{
    public class LeaveFeedbackJob
    {
        private readonly IFeedbackService feedbackService;

        public LeaveFeedbackJob(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }

        public void Do(string sellerId, string buyerId, int orderId)
        {
            feedbackService.LeaveAutomaticFeedback(sellerId, buyerId, orderId);
            feedbackService.SaveFeedback();
        }
    }
}