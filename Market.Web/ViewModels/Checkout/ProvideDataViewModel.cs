using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class AccountInfoViewModel
    {
        public int Id { get; set; }
        [Required]
        public string SteamLogin { get; set; }

        [Required]
        public string SteamPassword { get; set; }

        [Required]
        [Compare("SteamPassword", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmSteamPassword { get; set; }
        public string SteamEmail { get; set; }
        public string AdditionalInformation { get; set; }
        [Required]
        public string ModeratorId { get; set; }
        [Required]
        public string BuyerId { get; set; }
        [Required]
        public string SellerId { get; set; }
    }
}