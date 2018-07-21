
using System;
using System.Collections.Generic;


namespace Market.Model.Models
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string Avatar32Path { get; set; }
        public string Avatar48Path { get; set; }
        public string Avatar96Path { get; set; }

        public int PositiveFeedbackCount
        {
            set; get;
        }

        public int NegativeFeedbackCount
        {
            set; get;
        }

        public int? Rating
        {
            get
            {
                return PositiveFeedbackCount - NegativeFeedbackCount;
            }
            private set
            {

            }
        }

        public int? AllFeedbackCount
        {
            get
            {
                return PositiveFeedbackCount + NegativeFeedbackCount;
            }
            private set
            {

            }
        }

        public double? PositiveFeedbackProcent
        {
            get
            {
                if (AllFeedbackCount != 0)
                {
                    double pos = Math.Round((double)(100 * PositiveFeedbackCount) / (PositiveFeedbackCount + NegativeFeedbackCount), 2);
                    return pos;
                }
                return 0;
            }
            private set
            {

            }
        }

        public double? NegativeFeedbackProcent
        {
            get
            {
                if (AllFeedbackCount != 0)
                {
                    double neg = Math.Round((double)(100 * NegativeFeedbackCount) / (PositiveFeedbackCount + NegativeFeedbackCount), 2);
                    return neg;
                }
                return 0;
            }
            private set
            {

            }
        }

        public int? SuccessOrderRate
        {

            get
            {
                
                
                return 0;
            }
            private set
            {

            }
        }


        public string Discription { get; set; }
        public bool IsOnline { get; set; }
        public string LockoutReason { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string Name { get; set; }

        public IList<Billing> Billings { get; set; } = new List<Billing>();
        public IList<Transaction> TransactionsAsReceiver { get; set; } = new List<Transaction>();
        public IList<Transaction> TransactionsAsSender { get; set; } = new List<Transaction>();

        public decimal Balance { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Message> MessagesAsSender { get; set; } = new List<Message>();
        public ICollection<Message> MessagesAsReceiver { get; set; } = new List<Message>();
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();

        public ICollection<Order> OrdersAsSeller { get; set; } = new List<Order>();
        public ICollection<Order> OrdersAsBuyer { get; set; } = new List<Order>();
        public ICollection<Order> OrdersAsMiddleman { get; set; } = new List<Order>();

        public ICollection<Feedback> FeedbacksMy { get; set; } = new List<Feedback>();
        public ICollection<Feedback> FeedbacksToOthers { get; set; } = new List<Feedback>();

        public ICollection<Dialog> DialogsAsCreator { get; set; } = new List<Dialog>();
        public ICollection<Dialog> DialogsAsСompanion { get; set; } = new List<Dialog>();

        public ICollection<Withdraw> Withdraws { get; set; } = new List<Withdraw>();
    }
}
