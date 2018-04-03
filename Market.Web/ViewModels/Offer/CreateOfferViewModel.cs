using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.ViewModels
{
    public class CreateOfferViewModel
    {
        [Display(Name = "Заголовок")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        [Required(ErrorMessage = "Введите заголовок")]
        public string Header { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Введите описание")]
        [StringLength(maximumLength: 500, MinimumLength = 1)]
        public string Discription { get; set; }

        [Required]
        [Display(Name = "Логин steam")]
        [Remote("IsSteamLoginExists", "Offer", ErrorMessage = "Этот логин уже использовался")]
        public string SteamLogin { get; set; }


        public bool SellerPaysMiddleman { get; set; }
        [Display(Name = "Игра")]
        public string Game { get; set; }

        
        public string[] FilterValues { get; set; }

        
        public string[] FilterItemValues { get; set; }

        [Display(Name = "Игра")]
        public IEnumerable<SelectListItem> Games { get; set; }
        public IEnumerable<SelectListItem> Filters { get; set; }
        public IEnumerable<SelectListItem> FilterItems { get; set; }
        public Dictionary<Model.Models.Filter, FilterItem> FilterPairs { get; set; }

        [Display(Name = "Отображаемая цена")]
        [Required(ErrorMessage = "Введите цену")]
        [DataType(DataType.Currency, ErrorMessage = "цена введена некорректно")]
        [Range(50, 1000000, ErrorMessage = "Цена должна быть от 50 до 1000000 долларов")]
        [RegularExpression(@"\d+(.\d{1,2})?", ErrorMessage = "цена введена некорректно")]
        public decimal Price { get; set; }

        public CreateOfferViewModel()
        {
            Games = new List<SelectListItem>();
            Filters = new List<SelectListItem>();
            FilterItems = new List<SelectListItem>();

        }
    }
}