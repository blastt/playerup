﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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


        public virtual int GameId { get; set; }
        public virtual Game Game { get; set; }

        public virtual Order Order { get; set; }
        public virtual string UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual IList<Filter> Filters { get; set; } = new List<Filter>();
        public virtual IList<FilterItem> FilterItems { get; set; } = new List<FilterItem>();
    }
}
