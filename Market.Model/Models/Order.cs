using System;
using System.Collections.Generic;

namespace Market.Model.Models
{
    public enum Status
    {
        
        OrderCreated,
        SellerProviding,
        AdminChecking,
        BuyerConfirming,        
        PayingToSeller,
        ClosedSeccessfuly, // Успешно закрыт
        ColsedFelure, // Неудачно закрыт
        BuyerCancelledEarly, // Покупатель закрыл сделку
        SellerCancelledEarly, // Продавец закрыл сделку
        AdminCancelledEarly // Гарант закрыл сделку

    }
    public class Order
    {
        public int Id { get; set; }
        public bool BuyerFeedbacked { get; set; }
        public bool SellerFeedbacked { get; set; }

        public bool BuyerChecked { get; set; }
        public bool SellerChecked { get; set; }

        public string JobId { get; set; } // id задачи

        public decimal Sum { get; set; } // сумма заказа
        

        public decimal? WithmiddlemanSum // сумма с учетом стоимости гаранта
        {
            get
            {
                decimal middlemanPrice = 0;

                if (Sum < 3000)
                {
                    middlemanPrice = 300;

                }
                else if (Sum < 15000)
                {
                    middlemanPrice = Sum * Convert.ToDecimal(0.1);
                }
                else
                {
                    middlemanPrice = 1500;
                }

                return Sum - middlemanPrice;
            }
            private set
            {
                
            }
            
        }

        public decimal? Amount { get; set; } // сумма, которую заплатали с учетом комиссии
        public decimal? WithdrawAmount { get; set; } // сумма, которую заплатали без учета комиссии

        public decimal? AmmountSellerGet { get; set; } // сумма, которую заплатали с учетом комиссии
        public decimal? WithdrawAmountSellerGet { get; set; } // сумма, которую заплатали без учета комиссии

        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public Offer Offer { get; set; }

        public int CurrentStatusId { get; set; }
        public OrderStatus CurrentStatus { get; set; }
        public LinkedList<StatusLog> StatusLogs { get; set; } = new LinkedList<StatusLog>();

        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();

        public string MiddlemanId { get; set; }
        public UserProfile Middleman { get; set; }
        public string BuyerId { get; set; }
        public UserProfile Buyer { get; set; }
        public string SellerId { get; set; }
        public UserProfile Seller { get; set; }


        public IList<AccountInfo> AccountInfos { get; set; } = new List<AccountInfo>();

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
