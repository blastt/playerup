using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Web.ViewModels
{
    public class DetailsOfferViewModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public Game Game { get; set; }
        public string Discription { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int Views { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        public IList<ScreenshotPath> ScreenshotPathes { get; set; } = new List<ScreenshotPath>();

        public bool SellerPaysMiddleman { get; set; }

        [DataType(DataType.Currency)]
        public decimal MiddlemanPrice { get; set; }

        public string UserName { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public UserProfile UserProfile { get; set; }

        public IEnumerable<Filter> Filters { get; set; }
        public IEnumerable<FilterItem> FilterItems { get; set; }
        public IEnumerable<FeedbackViewModel> Feedbacks { get; set; }
    }
}