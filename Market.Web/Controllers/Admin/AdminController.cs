using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers.Admin
{
    public class AdminController : Controller
    {
        // GET: Admin
        //public ActionResult Panel()
        //{
        //    return View();
        //}
        private readonly IUserProfileService _userProfileService;
        private readonly IOfferService _offerService;
        private readonly IGameService _gameService;
        private readonly IFilterService _filterService;
        private readonly IFilterItemService _filterItemService;
        private readonly int offerDays = 30;
        public int pageSize = 4;
        public int pageSizeInUserInfo = 10;
        public AdminController(IOfferService offerService, IGameService gameService, IFilterService filterService, IFilterItemService filterItemService, IUserProfileService userProfileService)
        {
            _offerService = offerService;
            _gameService = gameService;
            _filterService = filterService;
            _filterItemService = filterItemService;
            _userProfileService = userProfileService;
        }

        public ActionResult OfferList()
        {
            OfferListViewModel model = new OfferListViewModel()
            {
                Offers = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(_offerService.GetOffers())
            };
            return View(model);
        }

        public ActionResult GameList()
        {
            IEnumerable<GameViewModel> model = Mapper.Map<IEnumerable<Game>, IEnumerable<GameViewModel>>(_gameService.GetGames());

            return View(model);
        }

        public ActionResult CreateGame()
        {            
            return View();
        }

        public ActionResult FilterList()
        {
            IEnumerable<FilterViewModel> model = Mapper.Map<IEnumerable<Model.Models.Filter>, IEnumerable<FilterViewModel>>(_filterService.GetFilters());

            return View(model);
        }

        public ActionResult FilterItemList()
        {
            IEnumerable<FilterItemViewModel> model = Mapper.Map<IEnumerable<FilterItem>, IEnumerable<FilterItemViewModel>>(_filterItemService.GetFilterItems());

            return View(model);
        }

        public ActionResult DeleteOffer(int id)
        {
            Offer offer = _offerService.GetOffer(id);
            if (offer != null)
            {
                _offerService.Delete(offer);
                TempData["message"] = string.Format("Предложение удалено");
                _offerService.SaveOffer();
            }
            return RedirectToAction("OfferList");
        }


    }
}