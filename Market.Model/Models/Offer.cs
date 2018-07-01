using System;
using System.Collections.Generic;


namespace Market.Model.Models
{
    public enum OfferState
    {
        active,
        inactive,
        closed
    }

    public class Offer
    {
        public int Id { get; set; }
        public string Header { get; set; }

        public string Discription { get; set; }
        public string AccountLogin { get; set; }

        public string JobId { get; set; } // id задачи

        public OfferState State { get; set; }

        public decimal Price { get; set; }

        public int Views { get; set; }

        public bool SellerPaysMiddleman { get; set; }

        public virtual IList<ScreenshotPath> ScreenshotPathes { get; set; } = new List<ScreenshotPath>();

        public decimal? MiddlemanPrice
        {
            get
            {
                decimal middlemanPrice = 0;

                if (Price < 3000)
                {
                    middlemanPrice = 300;

                }
                else if (Price < 15000)
                {
                    middlemanPrice = Price * Convert.ToDecimal(0.1);
                }
                else
                {
                    middlemanPrice = 1500;
                }

                return middlemanPrice;
            }

            private set
            {

            }
        }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateDeleted { get; set; }


        public int GameId { get; set; }
        public Game Game { get; set; }

        public Order Order { get; set; }
        public string UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public IList<Filter> Filters { get; set; } = new List<Filter>();
        public IList<FilterItem> FilterItems { get; set; } = new List<FilterItem>();
    }
}
