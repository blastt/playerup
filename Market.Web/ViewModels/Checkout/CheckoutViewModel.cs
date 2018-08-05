using System.ComponentModel.DataAnnotations;

namespace Market.Web.ViewModels
{
    public class CheckoutViewModel
    {
        public int OfferId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public string Game { get; set; }
        public string OfferHeader { get; set; }
        public bool SellerPaysMiddleman { get; set; }
        public bool UserCanPayWithBalance { get; set; }
        [DataType(DataType.Currency)]
        public decimal MiddlemanPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal OrderSum { get; set; }
        public string SellerId { get; set; }
        public string BuyerId { get; set; }
    }
}