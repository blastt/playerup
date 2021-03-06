﻿using System.ComponentModel.DataAnnotations;

namespace Market.Web.ViewModels
{
    public class AccountInfoViewModel
    {
        [Required]
        public string SteamLogin { get; set; }

        [Required]
        public string SteamPassword { get; set; }

        [Required]
        [Compare("SteamPassword", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmSteamPassword { get; set; }
        public string SteamEmail { get; set; }

        public string EmailPassword { get; set; }
        [Compare("EmailPassword", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmEmailPassword { get; set; }

        public string AdditionalInformation { get; set; }
        [Required]
        public string ModeratorId { get; set; }
        [Required]
        public string BuyerId { get; set; }
        [Required]
        public string SellerId { get; set; }

        public int OrderId { get; set; }
    }
}