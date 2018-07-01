namespace Market.Model.Models
{
    public class ScreenshotPath
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public int OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
