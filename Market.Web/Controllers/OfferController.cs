using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public int pageSize = 6;
        public OfferController(IOfferService offerService, IGameService gameService, IFilterService filterService, IFilterItemService filterItemService, IUserProfileService userProfileService)
        {
            _offerService = offerService;
            _gameService = gameService;
            _filterService = filterService;
            _filterItemService = filterItemService;
            _userProfileService = userProfileService;
        }

        // GET: Offer
        public ViewResult Buy(string game)
        {
            
            return View((object)game);
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

        public PartialViewResult List(SearchViewModel searchInfo)
        {
            decimal minGamePrice = 0;
            decimal maxGamePrice = 0;
            
            searchInfo.SearchString = searchInfo.SearchString ?? "";
            searchInfo.Game = searchInfo.Game ?? "all";
            //offer.Header.Replace(" ", "").ToLower().Contains(searchString.Replace(" ", "").ToLower()) || (searchInDescription ? (offer.Discription.Replace(" ", "").ToLower().Contains(searchString.Replace(" ", "").ToLower())) : searchInDescription)
            IEnumerable<Offer> offers;
            if (searchInfo.Game == "all")
            {
                offers = _offerService.GetOffers();
            }
            else
            {
                offers = _offerService.GetOffers().Where(m => m.Game.Value == searchInfo.Game);
            }
            offers = offers.Where(o => o.Order == null && o.State == OfferState.active);
            if (offers.Count() != 0)
            {
                minGamePrice = offers.Min(m => m.Price);

                maxGamePrice = offers.Max(m => m.Price);


                if (searchInfo.PriceFrom == 0)
                {
                    searchInfo.PriceFrom = minGamePrice;


                }
                if (searchInfo.PriceTo == 0)
                {
                    searchInfo.PriceTo = maxGamePrice;

                }




                offers = from offer in offers
                         where offer.Price >= searchInfo.PriceFrom - 1 &&
                                offer.Price <= searchInfo.PriceTo + 1
                         select offer;
            }

            if (searchInfo.SearchInDiscription)
            {
                offers = offers.Where(o => o.Header.Replace(" ", "").ToLower().Contains(searchInfo.SearchString.Replace(" ", "").ToLower()) || o.Discription.Replace(" ", "").ToLower().Contains(searchInfo.SearchString.Replace(" ", "").ToLower()));
            }
            else
            {
                offers = offers.Where(o => o.Header.Replace(" ", "").ToLower().Contains(searchInfo.SearchString.Replace(" ", "").ToLower()));
            }
            if (searchInfo.IsOnline)
            {
                offers = offers.Where(o => o.UserProfile.IsOnline);
            }
            
            
           
            
            switch (searchInfo.Sort)
            {
                case "bestSeller":
                    {
                        offers = from offer in offers
                                      orderby offer.UserProfile.Rating descending
                                      select offer;
                        break;
                    }
                case "priceDesc":
                    {
                        offers = from offer in offers
                                      orderby offer.Price descending
                                      select offer;
                        break;
                    }
                case "priceAsc":
                    {
                        offers = from offer in offers
                                      orderby offer.Price ascending
                                      select offer;
                        break;
                    }
                case "newestOffer":
                    {
                        offers = from offer in offers
                                      orderby offer.DateCreated descending
                                      select offer;
                        break;
                    }
                default:
                    break;
            }
            IList<SelectListItem> ranks = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Все ранги", Value = "none", Selected = true }
            };
            IList<OfferViewModel> offerList = new List<OfferViewModel>();
            var offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offers);

            var games = _gameService.GetGames();
            var gameViewModels = Mapper.Map<IEnumerable<Game>, IEnumerable<GameViewModel>>(games);
            //foreach (var offer in offers)
            //{
            //    offerList.Add(Mapper.Map<Offer, OfferViewModel>(offer));
            //}
            //Dictionary<Model.Models.Filter, FilterItem> offerFilters = new Dictionary<Model.Models.Filter, FilterItem>();
            //Dictionary<string, string> modelFilters = new Dictionary<string, string>();
            //if(searchInfo.FilterValues != null && searchInfo.FilterItemValues != null)
            //{
            //    if (searchInfo.FilterValues.Count() == searchInfo.FilterItemValues.Count())
            //    {
            //        for (int i = 0; i < searchInfo.FilterValues.Count(); i++)
            //        {
            //            modelFilters.Add(searchInfo.FilterValues[i], searchInfo.FilterItemValues[i]);
            //        }
            //    }
            //}

            //foreach (var offer in offerList)
            //{
            //    if(offer.FilterItems.Count == offer.Filters.Count)
            //    {
            //        for (int i = 0; i < offer.Filters.Count; i++)
            //        {
            //            offerFilters.Add(offer.Filters[i], offer.FilterItems[i]);
            //        }
            //    }
            //    var q = from f in offerFilters
            //            join fs in modelFilters on f.Value.Value equals fs.Value into qqq
            //            select new { filterValue = f.Value.Value, filterName = f.Value.Name, filterItemValue = f.Key.Value, filterItemName = f.Key.Text };
            //    offerFilters.Clear();
            //}


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
                
                Games = gameViewModels,
                Offers = offerViewModels.Skip((searchInfo.Page - 1) * pageSize).Take(pageSize).ToList(),
                SearchInfo = new SearchViewModel()
                {
                    SearchString = searchInfo.SearchString,
                    IsOnline = searchInfo.IsOnline,           
                    SearchInDiscription = searchInfo.SearchInDiscription,
                    MinGamePrice = minGamePrice,
                    MaxGamePrice = maxGamePrice,
                    PriceFrom = searchInfo.PriceFrom,
                    PriceTo = searchInfo.PriceTo,
                    Game = searchInfo.Game,
                    Page = 1,
                    Sort = searchInfo.Sort
                },
                PageInfo = new PageInfoViewModel()
                {
                    PageNumber = searchInfo.Page,
                    PageSize = pageSize,
                    TotalItems = offerViewModels.Count()
                }
            };

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
                Offers = modelOffers.Skip((searchInfo.Page - 1) * pageSize).Take(pageSize).ToList(),
                PageInfo = new PageInfoViewModel
                {
                    PageSize = pageSize,
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
                if(appUser != null)
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



            offer.MiddlemanPrice = 0;
            
            if (model.Price < 3000)
            {
                offer.MiddlemanPrice = 300;

            }
            else if (model.Price < 15000)
            {
                offer.MiddlemanPrice = model.Price * Convert.ToDecimal(0.1);
            }
            else
            {
                offer.MiddlemanPrice = 1500;
            }
            

            offer.Game = game;
            _filterService.SaveFilter();
            offer.UserProfile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            _offerService.CreateOffer(offer);
            _offerService.SaveOffer();

            return RedirectToAction("Buy");
        }

        public ActionResult Details(int? id = 2)
        {
            Offer offer = _offerService.GetOffer(id.Value);
            var model = Mapper.Map<Offer, DetailsOfferViewModel>(offer);
            return View(model);

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

            Offer offer = Mapper.Map<EditOfferViewModel, Offer>(model);

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



            offer.MiddlemanPrice = 0;

            if (model.Price < 3000)
            {
                offer.MiddlemanPrice = 300;

            }
            else if (model.Price < 15000)
            {
                offer.MiddlemanPrice = model.Price * Convert.ToDecimal(0.1);
            }
            else
            {
                offer.MiddlemanPrice = 1500;
            }


            offer.Game = game;
            _filterService.SaveFilter();
            offer.UserProfileId = User.Identity.GetUserId();
            _offerService.UpdateOffer(offer);
            _offerService.SaveOffer();

            return RedirectToAction("Buy");

        }

        //[HttpPost]
        //public ActionResult Edit(OfferViewModel model, string[] Filters)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool isGameExists = model.Games.Any(m => m.Value == model.Game);
        //        Offer currentOffer = _db.Offers.Find(m => m.SteamLogin == model.SteamLogin).FirstOrDefault();

        //        if (model.SteamLogin != _db.Offers.Get((model.Id).ToString()).SteamLogin)
        //        {
        //            return View(model);
        //        }
        //        if (currentOffer != null)
        //        {
        //            if (currentOffer.Id != model.Id)
        //            {
        //                if ((bool)IsSteamLoginExists(model.SteamLogin).Data)
        //                {
        //                    return View(model);
        //                }
        //            }
        //        }

        //        if (!ModelState.IsValid || !isGameExists)
        //        {
        //            return View(model);
        //        }
        //        UserProfile currentUserProfile = _db.UserProfiles.Get(User.Identity.GetUserId<string>());
        //        if (currentUserProfile != null)
        //        {
        //            var offer = new Offer
        //            {
        //                Game = model.Game,
        //                UserProfile = currentUserProfile,
        //                UserProfileId = currentUserProfile.Id,
        //                Header = model.Header,
        //                Discription = model.Discription,
        //                EndDate = DateTime.Now.AddDays(30),
        //                Price = model.Price,
        //                Filter = model.Filter,
        //                SteamLogin = model.SteamLogin,
        //                StartDate = DateTime.Now
        //            };
        //            ApplicationUser user = _db.Users.Get(User.Identity.GetUserId<string>());
        //            if (offer != null && user != null)
        //            {
        //                if (user.UserProfile.Offers.Contains(offer))
        //                {
        //                    return View(model);
        //                }

        //            }
        //            foreach (var filter in Filters)
        //            {
        //                offer.Filter += $"{filter},";
        //            }
        //            offer.Filter = offer.Filter.TrimEnd(',');

        //            _db.Offers.Update(offer);
        //            _db.Save();
        //        }
        //        return RedirectToAction("List");
        //    }
        //    return View(model);

        //}

        //public ActionResult Details(int? id = 1)
        //{
        //    Offer offer = _db.Offers.Get(id.ToString());
        //    ApplicationUser user;
        //    if (offer != null)
        //    {
        //        offer.Views++;
        //        user = _db.Users.Get(User.Identity.GetUserId<string>());
        //        FeedbackListViewModel feedbackListModel = new FeedbackListViewModel
        //        {
        //            Feedbacks = offer.UserProfile.Feedbacks
        //        };
        //        OfferViewModel model = new OfferViewModel()
        //        {
        //            Id = offer.Id,
        //            Discription = offer.Discription,
        //            EndDate = offer.EndDate,
        //            Filter = offer.Filter,
        //            Game = offer.Game,
        //            Header = offer.Header,
        //            Price = offer.Price,
        //            StartDate = offer.StartDate,
        //            SteamLogin = offer.SteamLogin,
        //            User = offer.UserProfile,
        //            Feedbacks = offer.UserProfile.Feedbacks,
        //            Views = offer.Views
        //        };
        //        _db.Save();
        //        return View(model);
        //        ViewData["Rating"] = offer.UserProfile.Rating;
        //        ViewData["Positive"] = offer.UserProfile.Positive;
        //        ViewData["Negative"] = offer.UserProfile.Negative;
        //        ViewData["IsOnline"] = offer.UserProfile.IsOnline;
        //        ViewData["Feedbacks"] = feedbackListModel;
        //        ViewData["UserName"] = offer.UserProfile.ApplicationUser.UserName;
        //        if (user != null)
        //        {
        //            ViewData["Offers"] = user.UserProfile.Offers;

        //        }
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }


        //}

        ////Сделать для EDIT
        //public JsonResult IsSteamLoginExists(string steamLogin)
        //{
        //    //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
        //    return Json(!_db.Offers.GetAll().Any(x => x.SteamLogin == steamLogin), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetMaxPrice()
        //{
        //    //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
        //    return Json(_db.Offers.GetAll().Max(m => m.Price), JsonRequestBehavior.AllowGet);
        //}

        //public void SetOnLineStatus()
        //{
        //    _offerService.SetOnLineStatus(User.Identity.GetUserId());
        //}

        //public void SetOffLineStatus()
        //{
        //    _offerService.SetOnLineStatus(User.Identity.GetUserId());
        //}

        //public JsonResult GetMinPrice()
        //{
        //    //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
        //    return Json(_db.Offers.GetAll().Min(m => m.Price), JsonRequestBehavior.AllowGet);
        //}

        public JsonResult IsSteamLoginExists(string steamLogin)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            bool exists = false;
            Offer offer = _offerService.GetOffers().FirstOrDefault(x => x.SteamLogin == steamLogin);
            if (offer != null)
            {
                if(offer.Order != null)
                {
                    OrderStatus orderStatus = offer.Order.OrderStatuses.OrderBy(m => m.DateFinished).LastOrDefault();
                    if (orderStatus != null && orderStatus.Value == "")
                    {
                        exists = false;
                        return Json(!exists, JsonRequestBehavior.AllowGet);
                    }
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
