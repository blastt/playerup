namespace Market.Model.Models
{
    public class AccountInfo
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public string AdditionalInformation { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
