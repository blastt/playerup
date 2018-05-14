using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set{}
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
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
        public ActionResult CreateGame(CreateGameViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var game = Mapper.Map<CreateGameViewModel, Game>(model);
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
                    var model = Mapper.Map<Game, EditGameViewModel>(game);
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditGame(EditGameViewModel model, HttpPostedFileBase image = null)
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
        [HttpGet]
        public ActionResult CreateFilter()
        {
            CreateFilterViewModel model = new CreateFilterViewModel();
            
            foreach (var game in _gameService.GetGames())
            {
                model.Games.Add(new SelectListItem { Value = game.Value, Text = game.Name });
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateFilter(CreateFilterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var game = _gameService.GetGameByValue(model.Game);
                var filter = Mapper.Map<CreateFilterViewModel, Model.Models.Filter>(model);
                filter.Game = game;
                _filterService.CreateFilter(filter);
                _filterService.SaveFilter();
                return RedirectToAction("FilterList");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditFilter(int? id)
        {
            if (id != null)
            {
                var filter = _filterService.GetFilter(id.Value);
                if (filter != null)
                {
                    var model = Mapper.Map<Model.Models.Filter, EditFilterViewModel>(filter);
                    foreach (var game in _gameService.GetGames())
                    {
                        model.Games.Add(new SelectListItem { Value = game.Value, Text = game.Name });
                    }
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditFilter(CreateFilterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filter = _filterService.GetFilter(model.Id);
                var game = _gameService.GetGameByValue(model.Game);
                if (filter != null && game != null)
                {
                    filter.Text = model.Name;
                    filter.Value = model.Value;
                    filter.Game = game;
                    //filter.Ra = model.Rank;
                    _filterService.SaveFilter();
                    return RedirectToAction("FilterItemList");
                }
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
        [HttpGet]
        public ActionResult CreateFilterItem()
        {
            CreateFilterItemViewModel model = new CreateFilterItemViewModel();
            foreach (var filter in _filterService.GetFilters())
            {
                model.Filters.Add(new SelectListItem()
                {
                    Text = string.Format($"{filter.Text} ({filter.Game.Name})"),
                    Value = filter.Value

                });
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateFilterItem(CreateFilterItemViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var filterItem = Mapper.Map<CreateFilterItemViewModel, Model.Models.FilterItem>(model);
                var filter = _filterService.GetFilterByValue(model.FilterValue);
                if (filter != null)
                {
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
                    filterItem.Filter = filter;
                    _filterItemService.CreateFilterItem(filterItem);
                    _filterItemService.SaveFilterItem();
                    return RedirectToAction("FilterItemList");
                }
                
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
                    var model = Mapper.Map<FilterItem, EditFilterItemViewModel>(filterItem);
                    foreach (var filter in _filterService.GetFilters())
                    {
                        model.Filters.Add(new SelectListItem()
                        {
                            Text = string.Format($"{filter.Text} ({filter.Game.Name})"),
                            Value = filter.Value

                        });
                    }
                    return View(model);
                }                                        
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditFilterItem(CreateFilterItemViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var filterItem = _filterItemService.GetFilterItem(model.Id);
                var filter = _filterService.GetFilterByValue(model.FilterValue);

                if (filterItem != null && filter != null)
                {
                    filterItem.Name = model.Name;
                    filterItem.Value = model.Value;
                    filterItem.Rank = model.Rank;
                    filterItem.Filter = filter;
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

        public ActionResult UserList()
        {
            IEnumerable<UserProfileViewModel> model = Mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserProfileViewModel>>(_userProfileService.GetUserProfiles());
            model = model.OrderBy(m => m.Name);
            return View(model);
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            if (id != null)
            {
                var user = _userProfileService.GetUserProfileById(id);
                if (user != null)
                {
                    var model = Mapper.Map<UserProfile, EditUserProfileViewModel>(user);
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditUser(EditUserProfileViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var user = _userProfileService.GetUserProfileById(model.Id);

                if (user != null)
                {
                    user.Name = model.Name;
                    user.ApplicationUser.UserName = model.Name;
                    user.ApplicationUser.Email = model.Email;
                    user.ApplicationUser.EmailConfirmed = model.EmailConfirmed;
                    user.ApplicationUser.PhoneNumber = model.PhoneNumber;
                    user.ApplicationUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    user.RegistrationDate = model.RegistrationDate;
                    user.Balance = model.Balance;
                    if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                    {
                        string extName = System.IO.Path.GetExtension(image.FileName);
                        string fileName = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);

                        // сохраняем файл в папку Files в проекте
                        string fullPath = Server.MapPath("~/Content/Images/FilterItems/" + fileName);
                        string urlPath = Url.Content("~/Content/Images/FilterItems/" + fileName);
                        image.SaveAs(fullPath);
                        user.ImagePath = urlPath;
                    }
                    _userProfileService.SaveUserProfile();
                    return RedirectToAction("FilterItemList");
                }
            }
            return HttpNotFound();
        }

        public async System.Threading.Tasks.Task<ActionResult> LockoutUserAsync(string id)
        {
            var result = await UserManager.SetLockoutEnabledAsync(id, true);
            TempData["message"] = string.Format("Ошибка");
            if (result.Succeeded)
            {
                
                var lockoutResult = await UserManager.SetLockoutEndDateAsync(id, DateTimeOffset.MaxValue);
                var user = UserManager.FindById(id);
                var updateStampResult = await UserManager.UpdateSecurityStampAsync(id);
                
                if (result.Succeeded && updateStampResult.Succeeded)
                {
                    TempData["message"] = string.Format("Пользователь заблокирован");
                }

            }
            return RedirectToAction("UserList");
        }

        public virtual async System.Threading.Tasks.Task<ActionResult> UnlockUserAccount(string id)
        {
            var result = UserManager.SetLockoutEnabledAsync(id, false);
            TempData["message"] = string.Format("Ошибка");
            if (result.Result.Succeeded)
            {
                await UserManager.ResetAccessFailedCountAsync(id);
                TempData["message"] = string.Format("Пользователь Разблокирован");
            }
            return RedirectToAction("UserList");
        }
    }
}