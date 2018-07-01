using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Web.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        public string LockoutReason { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        public string Avatar32Path { get; set; }
        public string Avatar48Path { get; set; }
        public string Avatar96Path { get; set; }

        public bool IsBanned { get; set; }

        public int Rating { get; set; }
        public double PositiveFeedbackProcent { get; set; }
        public double NegativeFeedbackProcent { get; set; }

        public int Positive { get; set; }
        public int Negative { get; set; }
    }
}