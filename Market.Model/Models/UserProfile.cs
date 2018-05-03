
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Model.Models
{
    public class UserProfile
    {
        public string Id { get; set; }
        public byte[] Avatar { get; set; }


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
                var ordersAsSeller = OrdersAsSeller.Where(o => o.CurrentStatus.Value == OrderStatuses.Feedbacking ||
                o.CurrentStatus.Value == OrderStatuses.ClosedSuccessfully);
                var ordersAsBuyer = OrdersAsBuyer.Where(o => o.CurrentStatus.Value == OrderStatuses.Feedbacking ||
                o.CurrentStatus.Value == OrderStatuses.ClosedSuccessfully);
                if (ordersAsSeller != null && ordersAsBuyer != null)
                {
                    return ordersAsBuyer.Count() + ordersAsSeller.Count();
                }
                
                return 0;
            }
            private set
            {

            }
        }


        public string Discription { get; set; }
        public bool IsOnline { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string Name { get; set; }

        public IList<Billing> Billings { get; set; } = new List<Billing>();
        public IList<BuyerInvoice> BuyerInvoices { get; set; } = new List<BuyerInvoice>();
        public IList<SellerInvoice> SellerInvoices { get; set; } = new List<SellerInvoice>();

        public decimal Balance { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Message> MessagesAsSender { get; set; } = new List<Message>();
        public virtual ICollection<Message> MessagesAsReceiver { get; set; } = new List<Message>();
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

        public virtual ICollection<Order> OrdersAsSeller { get; set; } = new List<Order>();
        public virtual ICollection<Order> OrdersAsBuyer { get; set; } = new List<Order>();
        public virtual ICollection<Order> OrdersAsMiddleman { get; set; } = new List<Order>();

        public virtual ICollection<Feedback> FeedbacksMy { get; set; } = new List<Feedback>();
        public virtual ICollection<Feedback> FeedbacksToOthers { get; set; } = new List<Feedback>();

        public virtual ICollection<Dialog> DialogsAsCreator { get; set; } = new List<Dialog>();
        public virtual ICollection<Dialog> DialogsAsСompanion { get; set; } = new List<Dialog>();
    }
}
