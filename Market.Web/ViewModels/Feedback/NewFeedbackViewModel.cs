using Market.Model.Models;

namespace Market.Web.ViewModels
{
    public class NewFeedbackViewModel
    {

        public Emotions Grade { get; set; }
        public string Comment { get; set; }


        public string OfferId { get; set; }

        public string OfferHeader { get; set; }

    }
}