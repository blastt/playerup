using Hangfire;
using Market.Service;

namespace Market.Web.Hangfire
{
    public class LeaveFeedbackJob
    {
        private readonly IFeedbackService feedbackService;

        public LeaveFeedbackJob(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }

        [DisableConcurrentExecution(10 * 60)]
        public void Do(string sellerId, string buyerId, int orderId)
        {
            feedbackService.LeaveAutomaticFeedback(sellerId, buyerId, orderId);
            feedbackService.SaveFeedback();
        }
    }
}