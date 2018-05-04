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

        public ActionResult GameList()
        {
            IEnumerable<GameViewModel> model = Mapper.Map<IEnumerable<Game>, IEnumerable<GameViewModel>>(_gameService.GetGames());

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateGame()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGame(GameViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var game = Mapper.Map<GameViewModel, Game>(model);
                if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                {
                    string extName = System.IO.Path.GetExtension(image.FileName);
                    string fileName = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);

                    // сохраняем файл в папку Files в проекте
                    string fullPath = Server.MapPath("~/Content/Images/FilterItems/" + fileName);
                    string urlPath = Url.Content("~/Content/Images/FilterItems/" + fileName);
                    image.SaveAs(fullPath);
                    game.ImagePath = urlPath;
                }
                _gameService.CreateGame(game);

                _gameService.SaveGame();
                return RedirectToAction("GameList");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditGame(int? id)
        {
            if (id != null)
            {
                var game = _gameService.GetGame(id.Value);
                if (game != null)
                {
                    var model = Mapper.Map<Game, GameViewModel>(game);
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditGame(GameViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var game = _gameService.GetGame(model.Id);
                if (game != null)
                {
                    game.Name = model.Name;
                    game.Value = model.Value;
                    game.Rank = model.Rank;
                    if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                    {
                        string extName = System.IO.Path.GetExtension(image.FileName);
                        string fileName = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);

                        // сохраняем файл в папку Files в проекте
                        string fullPath = Server.MapPath("~/Content/Images/FilterItems/" + fileName);
                        string urlPath = Url.Content("~/Content/Images/FilterItems/" + fileName);
                        image.SaveAs(fullPath);
                        game.ImagePath = urlPath;
                    }
                    _gameService.SaveGame();
                    return RedirectToAction("GameList");
                }
            }
            return HttpNotFound();
        }

        public ActionResult DeleteGame(int id)
        {
            Game game = _gameService.GetGame(id);
            if (game != null)
            {
                _gameService.Delete(game);
                TempData["message"] = string.Format("Игра удалена");
                _offerService.SaveOffer();
            }
            return RedirectToAction("GameList");
        }

        public ActionResult FilterList()
        {
            IEnumerable<FilterViewModel> model = Mapper.Map<IEnumerable<Model.Models.Filter>, IEnumerable<FilterViewModel>>(_filterService.GetFilters());

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateFilter(FilterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filter = Mapper.Map<FilterViewModel, Model.Models.Filter>(model);
                _filterService.CreateFilter(filter);
                _filterService.SaveFilter();
                return RedirectToAction("FilterList");
            }
            return HttpNotFound();
        }

        public ActionResult DeleteFilter(int id)
        {
            var filter = _filterService.GetFilter(id);
            if (filter != null)
            {
                _filterService.Delete(filter);
                TempData["message"] = string.Format("Игра удалена");
                _offerService.SaveOffer();
            }
            return RedirectToAction("GameList");
        }

        public ActionResult FilterItemList()
        {
            IEnumerable<FilterItemViewModel> model = Mapper.Map<IEnumerable<FilterItem>, IEnumerable<FilterItemViewModel>>(_filterItemService.GetFilterItems());
            model = model.OrderBy(m => m.Rank).OrderBy(m => m.FilterName).OrderBy(m => m.GameName);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateFilterItem(FilterItemViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var filterItem = Mapper.Map<FilterItemViewModel, Model.Models.FilterItem>(model);
                if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                {
                    string extName = System.IO.Path.GetExtension(image.FileName);
                    string fileName = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);

                    // сохраняем файл в папку Files в проекте
                    string fullPath = Server.MapPath("~/Content/Images/FilterItems/" + fileName);
                    string urlPath = Url.Content("~/Content/Images/FilterItems/" + fileName);
                    image.SaveAs(fullPath);
                    filterItem.ImagePath = urlPath;
                }
                _filterItemService.CreateFilterItem(filterItem);
                _filterItemService.SaveFilterItem();
                return RedirectToAction("FilterItemList");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditFilterItem(int? id)
        {
            if (id != null)
            {
                var filterItem = _filterItemService.GetFilterItem(id.Value);
                if (filterItem != null)
                {
                    var model = Mapper.Map<FilterItem, FilterItemViewModel>(filterItem);
                    return View(model);
                }                                        
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditFilterItem(FilterItemViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var filterItem = _filterItemService.GetFilterItem(model.Id);

                if (filterItem != null)
                {
                    filterItem.Name = model.Name;
                    filterItem.Value = model.Value;
                    filterItem.Rank = model.Rank;
                    if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                    {
                        string extName = System.IO.Path.GetExtension(image.FileName);
                        string fileName = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);

                        // сохраняем файл в папку Files в проекте
                        string fullPath = Server.MapPath("~/Content/Images/FilterItems/" + fileName);
                        string urlPath = Url.Content("~/Content/Images/FilterItems/" + fileName);
                        image.SaveAs(fullPath);
                        filterItem.ImagePath = urlPath;
                    }
                    _filterItemService.SaveFilterItem();
                    return RedirectToAction("FilterItemList");
                }
            }
            return HttpNotFound();
        }

        public ActionResult DeleteFilterItem(int id)
        {
            var filter = _filterService.GetFilter(id);
            if (filter != null)
            {
                _filterService.Delete(filter);
                TempData["message"] = string.Format("Игра удалена");
                _offerService.SaveOffer();
            }
            return RedirectToAction("GameList");
        }


    }
}