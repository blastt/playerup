using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class CreateWithdrawViewModel
    {
        [Required]
        public string PaywayName { get; set; }
        [Required]
        public string Details { get; set; }

        [Required(ErrorMessage = "Введите цену")]
        [DataType(DataType.Currency, ErrorMessage = "цена введена некорректно")]
        [Range(50, 100000, ErrorMessage = "Укажите сумму от 50 до 100000 рублей")]
        public decimal Amount { get; set; }
    }
}