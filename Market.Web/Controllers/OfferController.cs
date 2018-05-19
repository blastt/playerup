using AutoMapper;
using Hangfire;
using Market.Model.Models;
using Market.Service;
using Market.Web.Hangfire;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
        public int pageSize = 1;
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
        public async Task<ViewResult> Buy(SearchOffersInfoViewModel model)
        {
            var offers = await _offerService.GetOffersAsync(o => o.Game.Value == model.Game && o.State == OfferState.active);
            if (offers.Count() != 0)
            {
                model.PriceFrom = offers.Min(o => o.Price);
                model.PriceTo = offers.Max(o => o.Price);
            }
            

            model.Games = Mapper.Map<IEnumerable<Game>,IEnumerable<GameViewModel>>(_gameService.GetGames());
            return View(model);
        }

        public ViewResult Active()
        {
            IEnumerable<Offer> offersActive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.active);
            IEnumerable<Offer> offersInactive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.inactive);
            IEnumerable<Offer> offersClosed = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.closed);
            IEnumerable<OfferViewModel> offerViewModels;
            offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offersActive);
            OfferListViewModel model = new OfferListViewModel
            {
                Offers = offerViewModels,
                ActiveOffersCount = offersActive.Count(),
                InactiveOffersCount = offersInactive.Count(),
                CloseOffersCount = offersClosed.Count()
            };
            return View(model);
        }

        public ViewResult Inactive()
        {
            IEnumerable<Offer> offersActive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.active);
            IEnumerable<Offer> offersInactive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.inactive);
            IEnumerable<Offer> offersClosed = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.closed);
            IEnumerable<OfferViewModel> offerViewModels;
            offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offersInactive);
            OfferListViewModel model = new OfferListViewModel
            {
                Offers = offerViewModels,
                ActiveOffersCount = offersActive.Count(),
                InactiveOffersCount = offersInactive.Count(),
                CloseOffersCount = offersClosed.Count()
            };
            return View(model);
        }

        public ViewResult Closed()
        {
            IEnumerable<Offer> offersActive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.active);
            IEnumerable<Offer> offersInactive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.inactive);
            IEnumerable<Offer> offersClosed = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.closed);
            IEnumerable<OfferViewModel> offerViewModels;
            offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offersClosed);
            OfferListViewModel model = new OfferListViewModel
            {
                Offers = offerViewModels,
                ActiveOffersCount = offersActive.Count(),
                InactiveOffersCount = offersInactive.Count(),
                CloseOffersCount = offersClosed.Count()
            };
            return View(model);
        }
        private OfferListViewModel SearchOffers(SearchViewModel searchInfo)
        {


            searchInfo.SearchString = searchInfo.SearchString ?? "";
            searchInfo.Game = searchInfo.Game ?? "dota2";

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
            IEnumerable<Offer> foundOffers = _offerService.SearchOffers(game, sort, ref isOnline, ref searchInDiscription,
                searchString, ref page, pageSize, ref totalItems, ref minGamePrice, ref maxGamePrice, ref priceFrom, ref priceTo);
            IList<Offer> offers = new List<Offer>();
            if (searchInfo.JsonFilters.Count != 0)
            {
                bool equals = false;
                foreach (var offer in foundOffers)
                {
                    if (offer.FilterItems.Count == searchInfo.JsonFilters.Count)
                    {
                        for (int i = 0; i < offer.FilterItems.Count; i++)
                        {

                            if (offer.FilterItems[i].Value != searchInfo.JsonFilters[i].Value && searchInfo.JsonFilters[i].Value != "empty")
                            {
                                if (offer.Filters[i].Value == searchInfo.JsonFilters[i].Attribute)
                                {
                                    equals = false;
                                    break;
                                }


                            }

                            equals = true;
                        }
                        if (equals)
                        {
                            offers.Add(offer);
                        }
                        equals = false;
                    }
                }
            }

            IList<SelectListItem> ranks = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Все ранги", Value = "none", Selected = true }
            };
            IList<OfferViewModel> offerList = new List<OfferViewModel>();
            var offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offers);

            var games = _gameService.GetGames();
            var gameViewModels = Mapper.Map<IEnumerable<Game>, IEnumerable<GameViewModel>>(games);


            var filterDict = new Dictionary<Model.Models.Filter, FilterItem>();

            foreach (var offer in offerViewModels)
            {
                if (offer.Filters.Count() == offer.FilterItems.Count())
                {
                    for (int i = 0; i < offer.Filters.Count; i++)
                    {

                        filterDict.Add(offer.Filters[i], offer.FilterItems[i]);
                    }
                    offer.FilterFilterItem = filterDict;
                    filterDict = new Dictionary<Model.Models.Filter, FilterItem>();
                }
            }


            var model = new OfferListViewModel()
            {
                Filters = _filterService.GetFilters().Where(m => m.Game.Value == searchInfo.Game),
                Game = _gameService.GetGameByValue(searchInfo.Game),
                Offers = offerViewModels.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
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
                PageInfo = new PageInfoViewModel()
                {
                    
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = offers.Count()
                }
            };


            return model;

        }

        public PartialViewResult List(SearchViewModel searchInfo)
        {


            var model = SearchOffers(searchInfo);

            return PartialView("_OfferBlock", model);
        }

        public PartialViewResult Reset(SearchViewModel searchInfo)
        {
            var game = _gameService.GetGameByValue(searchInfo.Game);
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

            var model = SearchOffers(searchInfo);
            
            return PartialView("List", model);
        }

        public PartialViewResult OfferListInfo(SearchOffersInfoViewModel searchInfo)
        {
            searchInfo.SearchString = searchInfo.SearchString ?? "";
            var offers = _offerService.GetOffers().Where(m => m.UserProfileId == searchInfo.UserId);

            var modelOffers = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offers);
            IList<GameViewModel> gameList = new List<GameViewModel>();
            foreach (var offer in modelOffers)
            {
                GameViewModel game = new GameViewModel() { Name = offer.Game.Name, Value = offer.Game.Value };
                if (!gameList.Contains(game))
                {
                    gameList.Add(game);
                }

            }
            var games = gameList;
            modelOffers = modelOffers.Where(o => o.Header.Replace(" ", "").ToLower().Contains(searchInfo.SearchString.Replace(" ", "").ToLower()) || o.Discription.Replace(" ", "").ToLower().Contains(searchInfo.SearchString.Replace(" ", "").ToLower()));
            if (searchInfo.Game != null && searchInfo.Game != "all")
            {
                modelOffers = modelOffers.Where(m => m.Game.Value == searchInfo.Game);
            }

            OfferListViewModel model = new OfferListViewModel
            {
                Games = games,
                Offers = modelOffers.Skip((searchInfo.Page - 1) * pageSizeInUserInfo).Take(pageSizeInUserInfo).ToList(),
                PageInfo = new PageInfoViewModel
                {
                    PageSize = pageSizeInUserInfo,
                    PageNumber = searchInfo.Page,
                    TotalItems = modelOffers.Count()
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
            CreateOfferViewModel model = new CreateOfferViewModel();
            SelectList selectList = new SelectList(_gameService.GetGames());

            IList<SelectListItem> games = new List<SelectListItem>();
            foreach (var game in _gameService.GetGames())
            {
                games.Add(new SelectListItem { Value = game.Value, Text = game.Name });
            }

            model.Games = games;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateOfferViewModel model)
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userProfile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
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
            if (userProfile.Offers.Where(o => o.State == OfferState.active).Count() >= 10)
            {
                return View("CrateOfferLimitError");
            }
            Offer offer = Mapper.Map<CreateOfferViewModel, Offer>(model);
            
            Game game = _gameService.GetGameByValue(model.Game);
            var gameFilters = _filterService.GetFilters().Where(f => f.Game == game).ToList();
            var modelFilters = model.FilterValues;
            var gameFilterItems = _filterItemService.GetFilterItems().Where(f => f.Filter.Game == game).ToList();
            var modelFilterItems = model.FilterItemValues;
            if (game != null && modelFilters.Count() == gameFilters.Count())
            {
                for (int i = 0; i < gameFilters.Count; i++)
                {
                    if (gameFilters[i].Value != modelFilters[i])
                    {
                        return View("CreateOfferFiltersError");
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
                    isContainsFilterItems = false;

                }



            }
            else
            {
                return View("_CreateOfferFilterError");
            }


            offer.Game = game;
            offer.UserProfile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            offer.DateDeleted = offer.DateCreated.AddDays(offerDays);
            
            _offerService.CreateOffer(offer);
            _offerService.SaveOffer();
            
            offer.JobId = MarketHangfire.SetDeactivateOfferJob(offer.Id, TimeSpan.FromDays(30));

            _offerService.SaveOffer();

            return RedirectToAction("Buy");
        }

        public ActionResult Details(int? id = 1)
        {
            if (id != null)
            {
                Offer offer = _offerService.GetOffer(id.Value);
                var model = Mapper.Map<Offer, DetailsOfferViewModel>(offer);

                return View(model);
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
                var offer = _offerService.GetOffer(id.Value);

                if (offer.JobId != null)
                {
                    BackgroundJob.Delete(offer.JobId);
                    offer.JobId = null;
                }                
                _offerService.DeactivateOffer(offer, User.Identity.GetUserId());
                _offerService.SaveOffer();
            }
            return HttpNotFound();
        }
        [Authorize]
        public ActionResult Activate(int? id = 1)
        {
            if (id != null)
            {
                var offer = _offerService.GetOffer(id.Value);
                if (offer != null && offer.UserProfileId == User.Identity.GetUserId() && offer.State == OfferState.inactive)
                {
                    offer.State = OfferState.active;
                    offer.DateCreated = DateTime.Now;
                    offer.DateDeleted = offer.DateCreated.AddDays(offerDays);
                    _offerService.SaveOffer();

                    offer.JobId = MarketHangfire.SetDeactivateOfferJob(offer.Id, TimeSpan.FromDays(30));
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
                var offer = _offerService.GetOffer(id.Value);
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
            foreach (var game in _gameService.GetGames())
            {
                games.Add(new SelectListItem { Value = game.Value, Text = game.Name });
            }


            if (id != null)
            {
                Offer offer = _offerService.GetOffer(id.Value);
                if (offer != null && offer.UserProfileId == User.Identity.GetUserId())
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
        public ActionResult Edit(EditOfferViewModel model)
        {
            //EditOfferViewModel model = new EditOfferViewModel();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userProfile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
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
            var offer = _offerService.GetOffer(model.Id);
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
                var gameFilterItems = _filterItemService.GetFilterItems().Where(f => f.Filter.Game == game).ToList();
                var modelFilterItems = model.FilterItemValues;
                if (game != null && modelFilters.Count() == gameFilters.Count())
                {
                    offer.FilterItems.Clear();
                    offer.Filters.Clear();
                    for (int i = 0; i < gameFilters.Count; i++)
                    {
                        if (gameFilters[i].Value != modelFilters[i])
                        {
                            return View("CreateOfferFiltersError");
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
                        isContainsFilterItems = false;

                    }
                }
                else
                {
                    return View("_CreateOfferFilterError");
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
            Offer offer = _offerService.GetOffers().FirstOrDefault(x => x.AccountLogin == steamLogin);
            if (offer != null)
            {
                if (offer.Order != null)
                {                    
                    exists = false;
                    return Json(!exists, JsonRequestBehavior.AllowGet);                    
                }
                exists = true;
                return Json(!exists, JsonRequestBehavior.AllowGet);
            }
            return Json(!exists, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetFiltersJson(string game)
        {
            //Dictionary<string, string> ranks = new Dictionary<string, string>();
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            // Сделать проверку на налл
            var json = new JavaScriptSerializer().Serialize(_gameService.GetGameByValue(game).Filters);
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
