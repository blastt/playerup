using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.ViewModels
{
    public class EditOfferViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Заголовок")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Заголовок должен быть не менее {2} и не более {1} символов")]
        [Required(ErrorMessage = "Введите заголовок")]
        public string Header { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Введите описание")]
        [StringLength(maximumLength: 500, MinimumLength = 1, ErrorMessage = "Заголовок должен быть не менее {2} и не более {1} символов")]
        public string Discription { get; set; }

        
        [Display(Name = "Логин игрового аккаунта")]
        //[Remote("IsSteamLoginExists", "Offer", ErrorMessage = "Этот логин уже использовался")]
        public string SteamLogin { get; set; }


        public bool SellerPaysMiddleman { get; set; }
        [Display(Name = "Игра")]
        public string Game { get; set; }


        public string[] FilterValues { get; set; }

        public IList<ScreenshotPath> ScreenshotPathes { get; set; } = new List<ScreenshotPath>(3);

        public string[] FilterItemValues { get; set; }

        [Display(Name = "Игра")]
        public IEnumerable<SelectListItem> Games { get; set; }
        public IEnumerable<SelectListItem> Filters { get; set; }
        public IEnumerable<SelectListItem> FilterItems { get; set; }
        public Dictionary<Model.Models.Filter, FilterItem> FilterPairs { get; set; }

        [Display(Name = "Отображаемая цена")]
        [Required(ErrorMessage = "Введите цену")]
        [DataType(DataType.Currency, ErrorMessage = "цена введена некорректно")]
        [Range(300, 1000000, ErrorMessage = "Цена должна быть от 300 до 1000000 рублей")]

        [RegularExpression(@"^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$", ErrorMessage = "цена введена некорректно")]
        public decimal Price { get; set; }

        public EditOfferViewModel()
        {
            Games = new List<SelectListItem>();
            Filters = new List<SelectListItem>();
            FilterItems = new List<SelectListItem>();

        }
    }
}