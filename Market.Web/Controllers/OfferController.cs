using AutoMapper;
using Hangfire;
using Market.Model.Models;
using Market.Service;
using Market.Web.Hangfire;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TinifyAPI;
using Exception = System.Exception;

namespace Market.Web.Controllers
{
    public class OfferController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IOfferService _offerService;
        private readonly IGameService _gameService;
        private readonly IFilterService _filterService;
        private readonly IFilterItemService _filterItemService;
        private readonly int offerDays = 30;
        public int pageSize = 8;
        public int pageSizeInUserInfo = 10;
        public OfferController(IOfferService offerService, IGameService gameService, IFilterService filterService, IFilterItemService filterItemService, IUserProfileService userProfileService)
        {
            _offerService = offerService;
            _gameService = gameService;
            _filterService = filterService;
            _filterItemService = filterItemService;
            _userProfileService = userProfileService;
        }

        // GET: Offer
        [HttpGet]
        public ViewResult Buy(SearchViewModel searchInfo)
        {

            var offers = _offerService.GetOffersAsNoTracking(o => o.Game.Value == searchInfo.Game && o.State == OfferState.active, i => i.Game).ToList();

            if (offers.Any())
            {
                var minPrice = offers.Min(o => o.Price);
                var maxPrice = offers.Max(o => o.Price);
                searchInfo.MinGamePrice = minPrice;
                searchInfo.MaxGamePrice = maxPrice;
                searchInfo.PriceFrom = minPrice;
                searchInfo.PriceTo = maxPrice;
            }

            var model = SearchOffers(searchInfo, null);

            var game = _gameService.GetGamesAsNoTracking(g => g.Value == searchInfo.Game, f => f.Filters, ff => ff.Filters.Select(fi => fi.FilterItems)).ToList().FirstOrDefault();

            IList<SelectFilter> selects = new List<SelectFilter>();
            //IList<IList<FilterItemViewModel>> listOfOptions = new List<IList<FilterItemViewModel>>();                      
            if (game != null)
            {
                foreach (var filter in game.Filters)
                {
                    var filterItems = filter.FilterItems.OrderBy(f => f.Rank).ToList();
                    var selectOptions = new List<FilterItemViewModel>();

                    foreach (var filterItem in filterItems)
                    {
                        selectOptions.Add(new FilterItemViewModel
                        {
                            Value = filterItem.Value,
                            Name = filterItem.Name,
                            ImagePath = filterItem.ImagePath,
                            FilterValue = filter.Value

                        });
                    }
                    selects.Add(new SelectFilter
                    {
                        FilterName = filter.Text,
                        FilterValue = filter.Value,
                        Options = selectOptions
                    });
                }
                searchInfo.GameName = game.Name;
            }
            model.SearchInfo.SelectsOptions = selects;

            model.Games = Mapper.Map<IEnumerable<Game>, IEnumerable<GameViewModel>>(_gameService.GetGamesAsNoTracking().OrderBy(g => g.Rank).ToList());
            return View(model);
        }

        [Authorize]
        public ViewResult Active()
        {
            var currentUserId = User.Identity.GetUserId();
            IEnumerable<Offer> offersActive = _offerService.GetOffers(m => m.UserProfileId == currentUserId && m.State == OfferState.active, o => o.Game).ToList();
            IEnumerable<Offer> offersInactive = _offerService.GetOffers(m => m.UserProfileId == currentUserId && m.State == OfferState.inactive, o => o.Game).ToList();
            IEnumerable<Offer> offersClosed = _offerService.GetOffers(m => m.UserProfileId == currentUserId && m.State == OfferState.closed, o => o.Game).ToList();
            var offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offersActive.ToList());
            OfferListViewModel model = new OfferListViewModel
            {
                Offers = offerViewModels.OrderByDescending(o => o.DateCreated),
                ActiveOffersCount = offersActive.Count(),
                InactiveOffersCount = offersInactive.Count(),
                CloseOffersCount = offersClosed.Count()
            };
            
            return View(model);
        }

        [Authorize]
        public ViewResult Inactive()
        {
            var currentUserId = User.Identity.GetUserId();
            IEnumerable<Offer> offersActive = _offerService.GetOffers(m => m.UserProfileId == currentUserId && m.State == OfferState.active, o => o.Game).ToList();
            IEnumerable<Offer> offersInactive = _offerService.GetOffers(m => m.UserProfileId == currentUserId && m.State == OfferState.inactive, o => o.Game).ToList();
            IEnumerable<Offer> offersClosed = _offerService.GetOffers(m => m.UserProfileId == currentUserId && m.State == OfferState.closed, o => o.Game).ToList();
            var offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offersInactive.ToList());
            OfferListViewModel model = new OfferListViewModel
            {
                Offers = offerViewModels.OrderByDescending(o => o.DateCreated),
                ActiveOffersCount = offersActive.Count(),
                InactiveOffersCount = offersInactive.Count(),
                CloseOffersCount = offersClosed.Count()
            };
            return View(model);
        }

        [Authorize]
        public ViewResult Closed()
        {
            var currentUserId = User.Identity.GetUserId();
            IEnumerable<Offer> offersActive = _offerService.GetOffers(m => m.UserProfileId == currentUserId && m.State == OfferState.active, o => o.Game).ToList();
            IEnumerable<Offer> offersInactive = _offerService.GetOffers(m => m.UserProfileId == currentUserId && m.State == OfferState.inactive, o => o.Game).ToList();
            IEnumerable<Offer> offersClosed = _offerService.GetOffers(m => m.UserProfileId == currentUserId && m.State == OfferState.closed, o => o.Game).ToList();
            var offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offersClosed.ToList());
            OfferListViewModel model = new OfferListViewModel
            {
                Offers = offerViewModels.OrderByDescending(o => o.DateCreated),
                ActiveOffersCount = offersActive.Count(),
                InactiveOffersCount = offersInactive.Count(),
                CloseOffersCount = offersClosed.Count()
            };
            return View(model);
        }
        private OfferListViewModel SearchOffers(SearchViewModel searchInfo, string[] filters)
        {
            if (filters == null)
            {
                filters = new string[0];
            }

            searchInfo.SearchString = searchInfo.SearchString ?? "";
            searchInfo.Game = searchInfo.Game ?? "csgo";

            decimal minGamePrice = 0;
            decimal maxGamePrice = 0;
            int page = searchInfo.Page;
            string sort = searchInfo.Sort;
            bool isOnline = searchInfo.IsOnline;
            bool searchInDiscription = searchInfo.SearchInDiscription;
            string searchString = searchInfo.SearchString;
            string game = searchInfo.Game;
            int totalItems = 0;
            decimal priceFrom = searchInfo.PriceFrom;
            decimal priceTo = searchInfo.PriceTo;



            //offer.Header.Replace(" ", "").ToLower().Contains(searchString.Replace(" ", "").ToLower()) || (searchInDescription ? (offer.Discription.Replace(" ", "").ToLower().Contains(searchString.Replace(" ", "").ToLower())) : searchInDescription)
            IQueryable<Offer> foundOffers = _offerService.SearchOffers(game, sort, ref isOnline, ref searchInDiscription,
                searchString, ref page, pageSize, ref totalItems, ref minGamePrice, ref maxGamePrice, ref priceFrom, ref priceTo, filters);
            if (searchInfo.IsOnline)
            {
                if (HttpRuntime.Cache["LoggedInUsers"] is Dictionary<string, DateTime> loggedOnUsers)
                {
                    foundOffers = foundOffers.Where(o => loggedOnUsers.Any(u => u.Key == o.UserProfile.Name));
                }
            }

            var offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(foundOffers.ToList());


            var filterDict = new Dictionary<Model.Models.Filter, FilterItem>();

            var viewModels = offerViewModels.ToList();
            foreach (var offer in viewModels)
            {
                if (offer.Filters.Count != offer.FilterItems.Count) continue;
                for (var i = 0; i < offer.Filters.Count; i++)
                {

                    filterDict.Add(offer.Filters[i], offer.FilterItems[i]);
                }
                offer.FilterFilterItem = filterDict;
                filterDict = new Dictionary<Model.Models.Filter, FilterItem>();
            }


            var model = new OfferListViewModel()
            {
                Filters = _filterService.GetFiltersAsNoTracking(m => m.Game.Value == searchInfo.Game, i => i.Game).ToList(),
                Game = _gameService.GetGameByValue(searchInfo.Game),
                Offers = viewModels.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                SearchInfo = new SearchViewModel()
                {
                    SearchString = searchString,
                    IsOnline = isOnline,
                    SearchInDiscription = searchInDiscription,
                    MinGamePrice = minGamePrice,
                    MaxGamePrice = maxGamePrice,
                    PriceFrom = priceFrom,
                    PriceTo = priceTo,
                    Game = game,
                    Page = 1,
                    Sort = sort
                },
                PageInfo = new PageInfoViewModel
                {

                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = viewModels.Count
                }
            };
            model.SearchInfo.SortItems = new List<SelectListItem>
            {
                new SelectListItem { Value = "bestSeller", Text = @"Лучшие продавцы" },
                new SelectListItem { Value = "priceDesc", Text = @"От дорогих к дешевым" },
                new SelectListItem { Value = "priceAsc", Text = @"От дешевых к дорогим" },
                new SelectListItem { Value = "newestOffer", Text = @"Самые новые" }
            };

            foreach (var item in model.SearchInfo.SortItems)
            {
                item.Selected = item.Value == searchInfo.Sort;
            }

            return model;

        }

        public PartialViewResult List(SearchViewModel searchInfo, string[] filters)
        {


            var model = SearchOffers(searchInfo, filters);

            return PartialView("List", model);
        }

        public PartialViewResult Reset(SearchViewModel searchInfo, string[] filters)
        {
            var game = _gameService.GetGameByValueAsNoTracking(searchInfo.Game, g => g.Filters);
            if (searchInfo.JsonFilters.Count == 0)
            {
                if (game != null)
                {
                    foreach (var filter in game.Filters)
                    {
                        searchInfo.JsonFilters.Add(new JsonFilter { Value = "empty", Attribute = filter.Value });
                    }
                }
            }


            var model = SearchOffers(searchInfo, filters);

            return PartialView("List", model);
        }

        public PartialViewResult OfferListInfo(SearchOffersInfoViewModel searchInfo)
        {
            searchInfo.SearchString = searchInfo.SearchString ?? "";
            var offers = _offerService.GetOffers(m => m.UserProfileId == searchInfo.UserId && m.State == OfferState.active, o => o.Game);

            var modelOffers = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offers.ToList());
            IList<GameViewModel> gameList = new List<GameViewModel>();
            var offerViewModels = modelOffers.ToList();
            foreach (var offer in offerViewModels)
            {
                GameViewModel game = new GameViewModel() { Name = offer.Game.Name, Value = offer.Game.Value };
                if (!gameList.Contains(game))
                {
                    gameList.Add(game);
                }

            }
            var games = gameList;
            modelOffers = offerViewModels.Where(o => o.Header.Replace(" ", "").ToLower().Contains(searchInfo.SearchString.Replace(" ", "").ToLower()) || o.Discription.Replace(" ", "").ToLower().Contains(searchInfo.SearchString.Replace(" ", "").ToLower()));
            if (searchInfo.Game != null && searchInfo.Game != "all")
            {
                modelOffers = modelOffers.Where(m => m.Game.Value == searchInfo.Game);
            }

            var viewModels = modelOffers.ToList();
            var model = new OfferListViewModel
            {
                Games = games,
                Offers = viewModels.Skip((searchInfo.Page - 1) * pageSizeInUserInfo).Take(pageSizeInUserInfo),
                PageInfo = new PageInfoViewModel
                {
                    PageSize = pageSizeInUserInfo,
                    PageNumber = searchInfo.Page,
                    TotalItems = viewModels.Count
                },
                SearchInfo = new SearchViewModel
                {
                    SearchString = searchInfo.SearchString,
                    Page = searchInfo.Page
                }
            };

            return PartialView("_OfferListInfo", model);
        }

        [Authorize]
        public ActionResult Create()
        {
            var currentUserId = User.Identity.GetUserId();
            var userProfile = _userProfileService.GetUserProfiles(u => u.Id == currentUserId, i => i.ApplicationUser).ToList().SingleOrDefault();
            var appUser = userProfile?.ApplicationUser;
            if (appUser == null) return HttpNotFound();
            if (!(appUser.EmailConfirmed))
            {
                return View("PhoneNumberRequest");
            }
            else
            {
                var model = new CreateOfferViewModel();
                IList<SelectListItem> games = new List<SelectListItem>();
                model.SellerPaysMiddleman = true;
                //games.Add(new SelectListItem { Value = "", Text = "Выберите игру", Selected = true, Disabled = tr });
                foreach (var game in _gameService.GetGames().OrderBy(g => g.Rank))
                {
                    games.Add(new SelectListItem { Value = game.Value, Text = game.Name });
                }

                model.Games = games;

                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public async System.Threading.Tasks.Task<ActionResult> Create(CreateOfferViewModel model, HttpPostedFileBase[] images)
        {

            Tinify.Key = ConfigurationManager.AppSettings["TINYPNG_APIKEY"];
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var currentUserId = User.Identity.GetUserId();
            var userProfile = _userProfileService.GetUserProfiles(u => u.Id == currentUserId, i => i.ApplicationUser).SingleOrDefault();
            if (userProfile != null)
            {
                var appUser = userProfile.ApplicationUser;
                if (appUser != null)
                {
                    if (!(appUser.EmailConfirmed))
                    {
                        return HttpNotFound("you are not confirmed email or phone number");
                    }
                }

            }
            else
            {
                return View("_CreateOfferConfirmationError");
            }
            if (userProfile.Offers.Count(o => o.State == OfferState.active) >= 10)
            {
                return View("CrateOfferLimitError");
            }
            Offer offer = Mapper.Map<CreateOfferViewModel, Offer>(model);
            
            Game game = _gameService.GetGameByValue(model.Game);
            var gameFilters = _filterService.GetFilters(f => f.Game.Value == game.Value, i => i.Game, i => i.FilterItems).ToList();
            var modelFilters = model.FilterValues;
            //var gameFilterItems = _filterItemService.GetFilterItems().Where(f => f.Filter.Game == game).ToList();
            var modelFilterItems = model.FilterItemValues;
            if (modelFilters != null && gameFilters.Count != 0)
            {
                if (game != null && modelFilters.Length == gameFilters.Count)
                {
                    for (int i = 0; i < gameFilters.Count; i++)
                    {
                        if (gameFilters[i].Value != modelFilters[i])
                        {
                            return View("_CreateOfferFilterError");
                        }

                        bool isContainsFilterItems = false;
                        foreach (var fItem in gameFilters[i].FilterItems)
                        {
                            if (fItem.Value == modelFilterItems[i])
                            {
                                offer.FilterItems.Add(fItem);
                                offer.Filters.Add(gameFilters[i]);
                                isContainsFilterItems = true;
                            }
                        }
                        if (!isContainsFilterItems)
                        {
                            return View("_CreateOfferFilterError");
                        }
                    }



                }
                else
                {
                    return View("_CreateOfferFilterError");
                }
            }


            foreach (var image in images)
            {
                if (image != null && image.ContentLength <= 1000000 && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                {
                    var extName = System.IO.Path.GetExtension(image.FileName);
                    var fileName = $@"{Guid.NewGuid()}{extName}";
                    // сохраняем файл в папку Files в проекте
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\Avatars", fileName);
                    var urlPath = Url.Content("~/Content/Images/Screenshots/" + fileName);
                    image.SaveAs(fullPath);


                    offer.ScreenshotPathes.Add(new ScreenshotPath { Value = urlPath });
                }
                else
                {
                    offer.ScreenshotPathes.Add(new ScreenshotPath { Value = null });
                }
            }

            offer.GameId = game.Id;
            offer.UserProfileId = _userProfileService.GetUserProfileById(User.Identity.GetUserId()).Id;
            offer.DateDeleted = offer.DateCreated.AddDays(offerDays);

            _offerService.CreateOffer(offer);
            _offerService.SaveOffer();

            if (Request.Url != null)
                offer.JobId = MarketHangfire.SetDeactivateOfferJob(offer.Id,
                    Url.Action("Activate", "Offer", new {id = offer.Id}, Request.Url.Scheme), TimeSpan.FromDays(30));

            _offerService.SaveOffer();
            var offerModel = Mapper.Map<Offer, OfferViewModel>(offer);
            return View("OfferCreated", offerModel);
        }

        public ActionResult Details(int? id = 1)
        {
            if (id != null)
            {
                Offer offer = _offerService.GetOffers(o => o.Id == id.Value, i => i.UserProfile,
                    i => i.Game, i => i.FilterItems, i => i.FilterItems.Select(fi => fi.Filter),
                    i => i.UserProfile.FeedbacksMy).SingleOrDefault();
                if (offer != null)
                {
                    offer.Views++;
                    _offerService.SaveOffer();
                    var model = Mapper.Map<Offer, DetailsOfferViewModel>(offer);

                    return View(model);
                }

            }
            return HttpNotFound();

        }


        //[Authorize]
        //[HttpPost]
        //public JsonResult DeleteAjax(int? id)
        //{
        //    ApplicationUser user = _db.Users.Get(User.Identity.GetUserId<string>());
        //    if (user != null)
        //    {
        //        bool isValidId = false;
        //        foreach (var offer in user.UserProfile.Offers)
        //        {
        //            if (offer.Id == id)
        //            {
        //                isValidId = true;
        //                break;
        //            }
        //        }
        //        if (!isValidId)
        //        {
        //            return Json(new { Success = false });
        //        }
        //    }
        //    _db.Offers.Delete(id.ToString());
        //    _db.Save();
        //    return Json(new { Success = true });
        //}
        [Authorize]
        public ActionResult Deactivate(int? id = 1)
        {
            if (id != null)
            {
                var offer = _offerService.GetOffers(o => o.Id == id.Value, o => o.UserProfile).SingleOrDefault();
                if (offer != null && offer.UserProfileId == User.Identity.GetUserId() && offer.State == OfferState.active)
                {
                    if (offer.JobId != null)
                    {
                        BackgroundJob.Delete(offer.JobId);
                        offer.JobId = null;
                    }
                    _offerService.DeactivateOffer(offer, User.Identity.GetUserId());
                    _offerService.SaveOffer();
                    return View();
                }
            }
            return HttpNotFound();
        }
        [Authorize]
        public ActionResult Activate(int? id = 1)
        {
            if (id != null)
            {
                var offer = _offerService.GetOffers(o => o.Id == id.Value, o => o.UserProfile).SingleOrDefault();
                if (offer != null && offer.UserProfileId == User.Identity.GetUserId() && offer.State == OfferState.inactive)
                {
                    offer.State = OfferState.active;
                    offer.DateCreated = DateTime.Now;
                    offer.DateDeleted = offer.DateCreated.AddDays(offerDays);
                    _offerService.SaveOffer();

                    if (Request.Url != null)
                        offer.JobId = MarketHangfire.SetDeactivateOfferJob(offer.Id,
                            Url.Action("Activate", "Offer", new {id = offer.Id}, Request.Url.Scheme),
                            TimeSpan.FromDays(30));
                    _offerService.SaveOffer();
                    return View();
                }
            }
            return HttpNotFound();
        }
        [Authorize]
        public ActionResult Delete(int? id = 1)
        {
            if (id != null)
            {
                var offer = _offerService.GetOffers(o => o.Id == id.Value, o => o.UserProfile).SingleOrDefault();
                if (offer != null && offer.UserProfileId == User.Identity.GetUserId() && offer.State == OfferState.inactive)
                {
                    _offerService.Delete(offer);
                    _offerService.SaveOffer();
                    return View();
                }
            }
            return HttpNotFound();
        }
        [Authorize]
        public ActionResult Edit(int? id = 1)
        {
            //EditOfferViewModel model = new EditOfferViewModel();


            IList<SelectListItem> games = new List<SelectListItem>();
            foreach (var game in _gameService.GetGames().OrderBy(g => g.Rank))
            {
                games.Add(new SelectListItem { Value = game.Value, Text = game.Name });
            }


            if (id != null)
            {
                var currentUserId = User.Identity.GetUserId();
                Offer offer = _offerService.GetOffers(o => o.Id == id.Value && o.UserProfileId == currentUserId, o => o.Game).SingleOrDefault();
                if (offer != null)
                {

                    var model = Mapper.Map<Offer, EditOfferViewModel>(offer);
                    model.Game = offer.Game.Value;
                    model.Games = games;

                    return View(model);

                }

            }
            return HttpNotFound();

        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(EditOfferViewModel model, HttpPostedFileBase[] images)
        {
            //EditOfferViewModel model = new EditOfferViewModel();

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Buy", new { id = model.Id });
            }
            var currentUserId = User.Identity.GetUserId();

            var userProfile = _userProfileService.GetUserProfiles(u => u.Id == currentUserId, i => i.ApplicationUser).SingleOrDefault();
            if (userProfile != null)
            {
                var appUser = userProfile.ApplicationUser;
                if (appUser != null)
                {
                    //if(!(appUser.PhoneNumberConfirmed && appUser.EmailConfirmed))
                    //{
                    //    return HttpNotFound("you are not confirmed email or phone number");
                    //}
                }

            }
            else
            {
                return View("_CreateOfferConfirmationError");
            }
            var offer = _offerService.GetOffer(model.Id, o => o.Game, o => o.Filters, o => o.FilterItems);

            if (offer.UserProfileId == User.Identity.GetUserId() && offer.State == OfferState.active)
            {
                
                Game game = _gameService.GetGameByValue(model.Game);
                offer.Price = model.Price;
                offer.SellerPaysMiddleman = model.SellerPaysMiddleman;
                offer.Game = game;
                offer.Discription = model.Discription;
                offer.Header = model.Header;


                var gameFilters = _filterService.GetFilters().Where(f => f.Game == game).ToList();
                var modelFilters = model.FilterValues;
                var modelFilterItems = model.FilterItemValues;
                if (game != null && modelFilters.Length == gameFilters.Count)
                {
                    offer.FilterItems.Clear();
                    offer.Filters.Clear();
                    for (int i = 0; i < gameFilters.Count; i++)
                    {
                        if (gameFilters[i].Value != modelFilters[i])
                        {
                            return View("_CreateOfferFilterError");
                        }

                        bool isContainsFilterItems = false;
                        foreach (var fItem in gameFilters[i].FilterItems)
                        {
                            if (fItem.Value == modelFilterItems[i])
                            {

                                offer.FilterItems.Add(fItem);
                                offer.Filters.Add(gameFilters[i]);
                                isContainsFilterItems = true;
                            }
                        }
                        if (!isContainsFilterItems)
                        {
                            return View("_CreateOfferFilterError");
                        }
                    }
                }
                else
                {
                    return View("_CreateOfferFilterError");
                }

                for (int i = 0; i < offer.ScreenshotPathes.Count; i++)
                {
                    try
                    {

                        if (images[i] != null && images[i].ContentLength <= 1000000 && (images[i].ContentType == "image/jpeg" || images[i].ContentType == "image/png"))
                        {
                            if (offer.ScreenshotPathes[i].Value != null)
                            {
                                System.IO.File.Delete(Server.MapPath(offer.ScreenshotPathes[i].Value));

                            }


                            var extName = System.IO.Path.GetExtension(images[i].FileName);
                            var fileName = $@"{Guid.NewGuid()}{extName}";
                            // сохраняем файл в папку Files в проекте
                            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\Screenshots", fileName);
                            var urlPath = Url.Content("~/Content/Images/Screenshots/" + fileName);
                            try
                            {
                                images[i].SaveAs(fullPath);
                            }
                            catch (Exception)
                            {
                                return HttpNotFound();
                            }
                            offer.ScreenshotPathes[i].Value = urlPath;
                        }

                    }
                    catch (Exception)
                    {
                        return HttpNotFound();
                    }

                }

                _offerService.SaveOffer();

                return RedirectToAction("Buy");
            }

            return HttpNotFound();
        }



        public JsonResult IsSteamLoginExists(string steamLogin)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            bool exists = false;
            Offer offer = _offerService.GetOffers(x => x.AccountLogin == steamLogin, o => o.Order).ToList().LastOrDefault();

            if (offer != null)
            {
                if (offer.Order != null)
                {
                    if (offer.Order.CurrentStatus.Value == OrderStatuses.BuyerClosed || offer.Order.CurrentStatus.Value == OrderStatuses.SellerClosed ||
                        offer.Order.CurrentStatus.Value == OrderStatuses.MiddlemanClosed)
                    {
                        exists = true;
                    }

                    return Json(!exists, JsonRequestBehavior.AllowGet);
                }

                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetFiltersJson(string game)
        {
            //Dictionary<string, string> ranks = new Dictionary<string, string>();
            // Сделать проверку на налл
            var json = new JavaScriptSerializer().Serialize(_gameService.GetGameByValueAsNoTracking(game, g => g.Filters).Filters);
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
