using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Web.ViewModels
{
    public class EditUserProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        public string ImagePath { get; set; }

        public bool IsBanned { get; set; }
    }
}