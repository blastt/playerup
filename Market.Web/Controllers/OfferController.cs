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
        public int pageSize = 3;
        public OfferController(IOfferService offerService, IGameService gameService, IFilterService filterService, IFilterItemService filterItemService, IUserProfileService userProfileService)
        {
            _offerService = offerService;
            _gameService = gameService;
            _filterService = filterService;
            _filterItemService = filterItemService;
            _userProfileService = userProfileService;
        }

        // GET: Offer
        public ViewResult List(string game)
        {

            IEnumerable<Game> games = _gameService.GetGames();

            OfferListViewModel model = new OfferListViewModel();
            model.Games = games;
            model.SearchInfo = new SearchViewModel()
            {
                Game = game
            };
            return View(model);
        }

        public ViewResult All()
        {
            IEnumerable<Offer> offers = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId());
            IEnumerable<OfferViewModel> offerViewModels;
            offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offers);
            OfferListViewModel model = new OfferListViewModel
            {
                Offers = offerViewModels
            };
            return View(model);
        }

        public PartialViewResult OfferList(SearchViewModel searchInfo)
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
                         where offer.Price >= searchInfo.PriceFrom &&
                                offer.Price <= searchInfo.PriceTo
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




            var model = new OfferListViewModel()
            {
                Filters = _filterService.GetFilters().Where(m => m.Game.Value == searchInfo.Game),
                Game = _gameService.GetGameByValue(searchInfo.Game),
                
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

            return PartialView("_OfferList", model);
        }

        //public ViewResult All()
        //{
        //    IEnumerable<Offer> offers = _db.Offers.Find(m => m.UserProfileId == User.Identity.GetUserId());
        //    OfferListViewModel model = new OfferListViewModel
        //    {
        //        Offers = new List<OfferViewModel>(),
        //    };
        //    foreach (var offer in offers)
        //    {
        //        model.Offers.Add(new OfferViewModel
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
        //            Views = offer.Views
        //        });
        //    }
        //    return View(model);
        //}

        // Change Status offer 
        // Create OfferStatus
        //public ViewResult Active()
        //{
        //    IEnumerable<Offer> offers = _db.Offers.Find(m => m.UserProfileId == User.Identity.GetUserId()).Where(m => m.Order == null);
        //    OfferListViewModel model = new OfferListViewModel
        //    {
        //        Offers = new List<OfferViewModel>(),
        //    };
        //    foreach (var offer in offers)
        //    {
        //        model.Offers.Add(new OfferViewModel
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
        //            Views = offer.Views
        //        });
        //    }
        //    return View(model);
        //}

        // Change Status offer 
        // Create OfferStatus
        //public ViewResult Closed()
        //{
        //    IEnumerable<Offer> offers = _db.Offers.Find(m => m.UserProfileId == User.Identity.GetUserId()).Where(m => m.Order != null);
        //    OfferListViewModel model = new OfferListViewModel
        //    {
        //        Offers = new List<OfferViewModel>(),
        //    };
        //    foreach (var offer in offers)
        //    {
        //        model.Offers.Add(new OfferViewModel
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
        //            Views = offer.Views
        //        });
        //    }
        //    return View(model);
        //}
        //public PartialViewResult OfferList(OfferListViewModel model)
        //{
        //    // parse filter array to string for storing it in the database



        //    IEnumerable<Offer> offers = _offerService.GetOffers().Where(o => o.Game.Value == model.Game.Value).Where(o => o);


        //    //if (searchInfo.IsOnline)
        //    //{
        //    //    offers = from offer in offers
        //    //             where offer.UserProfile.IsOnline == true
        //    //             select offer;
        //    //}
        //    //if (searchInfo.MaxPrice != 0)
        //    //{
        //    //    offers = from offer in offers
        //    //             where offer.Price >= searchInfo.MinPrice &&
        //    //                    offer.Price <= searchInfo.MaxPrice
        //    //             select offer;
        //    //}


        //    //if (!string.IsNullOrEmpty(searchInfo.SearchString))
        //    //{
        //    //    offers = _offerService.Search(searchInfo.SearchString, searchInfo.SearchInDescription, offers);
        //    //}

        //    //var model = new OfferListViewModel
        //    //{
        //    //    Offers = new List<OfferViewModel>(),
        //    //    //Offers = offers.Skip((searchInfo.Page - 1) * pageSize).Take(pageSize),
        //    //    SearchingInfo = searchInfo,
        //    //    PagingInfo = new PagingInfo
        //    //    {
        //    //        CurrentPage = searchInfo.Page,
        //    //        PageSize = pageSize,
        //    //        TotalItems = offers.Count()
        //    //    }
        //    //};
        //    foreach (var offer in offers)
        //    {
        //        model.Offers.Add(new OfferViewModel
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
        //            Views = offer.Views
        //        });
        //    }
        //    //model.Offers = model.Offers.Skip((searchInfo.Page - 1) * pageSize).Take(pageSize).ToList();
        //    return PartialView("_OfferList", model);

        // Get Ajax offer list
        //public PartialViewResult OfferList(Game game, string[] Filters = null)
        //{
        //    // parse filter array to string for storing it in the database
        //    if (Filters != null)
        //    {
        //        foreach (var filter in Filters)
        //        {
        //            searchInfo.Filter += $"{filter},";
        //        }
        //        searchInfo.Filter = searchInfo.Filter.TrimEnd(',');
        //    }


        //    IEnumerable<Offer> offers = _db.Offers.GetAll();
        //    if (searchInfo.Game != "all")
        //    {
        //        offers = offers.Where(m => m.Game == searchInfo.Game);
        //        offers = _offerService.Filter(searchInfo.Filter, searchInfo.Game, offers);
        //    }
        //    offers = _offerService.OrderBy(searchInfo.OrderBy, offers);


        //    if (searchInfo.IsOnline)
        //    {
        //        offers = from offer in offers
        //                 where offer.UserProfile.IsOnline == true
        //                 select offer;
        //    }
        //    if (searchInfo.MaxPrice != 0)
        //    {
        //        offers = from offer in offers
        //                 where offer.Price >= searchInfo.MinPrice &&
        //                        offer.Price <= searchInfo.MaxPrice
        //                 select offer;
        //    }


        //    if (!string.IsNullOrEmpty(searchInfo.SearchString))
        //    {
        //        offers = _offerService.Search(searchInfo.SearchString, searchInfo.SearchInDescription, offers);
        //    }

        //    var model = new OfferListViewModel
        //    {
        //        Offers = new List<OfferViewModel>(),
        //        //Offers = offers.Skip((searchInfo.Page - 1) * pageSize).Take(pageSize),
        //        SearchingInfo = searchInfo,
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = searchInfo.Page,
        //            PageSize = pageSize,
        //            TotalItems = offers.Count()
        //        }
        //    };
        //    foreach (var offer in offers)
        //    {
        //        model.Offers.Add(new OfferViewModel
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
        //            Views = offer.Views
        //        });
        //    }
        //    model.Offers = model.Offers.Skip((searchInfo.Page - 1) * pageSize).Take(pageSize).ToList();
        //    return PartialView("_OfferList", model);
        //}

        //public ActionResult Buy()
        //{
        //    var gameList = new OfferViewModel().Games.Skip(1);
        //    return View(gameList);
        //}

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
            IList<SelectListItem> filters = new List<SelectListItem>();
            foreach (var filter in _filterService.GetFilters())
            {
                filters.Add(new SelectListItem { Value = filter.Value, Text = filter.Text });
            }
            IList<SelectListItem> filterItems = new List<SelectListItem>();
            foreach (var filterItem in _filterItemService.GetFilterItems())
            {
                filterItems.Add(new SelectListItem { Value = filterItem.Value, Text = filterItem.Name });
            }
            model.Games = games;
            model.Filters = filters;
            model.FilterItems = filterItems;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateOfferViewModel model, string test)
        {

            Game game = _gameService.GetGameByValue(model.Game);
            Offer offer = Mapper.Map<CreateOfferViewModel, Offer>(model);
            offer.Game = game;
            offer.Filters = new List<Model.Models.Filter>();
            offer.FilterItems = new List<FilterItem>();
            offer.UserProfile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            if(model.FilterValues != null)
            {
                foreach (var filterValue in model.FilterValues)
                {
                    var filter = _filterService.GetFilters().Where(m => m.Value == filterValue).FirstOrDefault();
                    offer.Filters.Add(filter);
                    _filterService.SaveFilter();
                }
            }

            if (model.FilterItemValues != null)
            {
                foreach (var filterItemValue in model.FilterItemValues)
                {
                    var filterItem = _filterItemService.GetFilterItems().Where(m => m.Value == filterItemValue).FirstOrDefault();
                    offer.FilterItems.Add(filterItem);
                    _filterItemService.SaveFilterItem();
                }
            }
            _offerService.CreateOffer(offer);
            _offerService.SaveOffer();
            //var q2 = from item in offer.FilterItems
            //         join filter in offer.Filters on item. equals filter.Id
            //         select new { itemValue = item.Value, filterValue = filter.Value };






            //if(game.Filters.Count() == model.FilterValues.Count() && game != null)
            //{
            //    foreach (var filter in game.Filters)
            //    {
            //        var contains = model.FilterValues.Contains(filter.Value);
            //        if (!contains)
            //        {
            //            return HttpNotFound("Filter is not found!");
            //        }
            //        else
            //        {
            //            var f = game.Filters.FirstOrDefault(m => m.Value == filter.Value);

            //            offer.Filters.Add(f,f.FilterItems.Where(m => m.Value == ));

            //        }
            //    }
            //    foreach (var item in model.FilterItemValues)
            //    {
            //        var fitems = _filterItemService.GetFilterItems().Where(m => m.Value == item).FirstOrDefault(); ;

            //        offer.FilterItems = fitems.ToList();
            //    }

            //}



            //bool isGameExists = model.Games.Any(m => m.Value == model.Game);

            //if (!ModelState.IsValid || !isGameExists || !(bool)(IsSteamLoginExists(model.SteamLogin).Data))
            //{
            //    return View(model);
            //}
            //Offer offer = Mapper.Map<CreateOfferViewModel, Offer>(model);
            //UserProfile currentUserProfile = _db.UserProfiles.Get(User.Identity.GetUserId<string>());
            //if (currentUserProfile != null)
            //{
            //    var offer = new Offer
            //    {
            //        Game = model.Game,
            //        UserProfile = currentUserProfile,
            //        UserProfileId = currentUserProfile.Id,
            //        Header = model.Header,
            //        Discription = model.Discription,
            //        EndDate = DateTime.Now.AddDays(30),
            //        Price = model.Price,
            //        Filter = model.Filter,
            //        SteamLogin = model.SteamLogin,
            //        StartDate = DateTime.Now
            //    };
            //    foreach (var filter in Filters)
            //    {
            //        offer.Filter += $"{filter},";
            //    }
            //    offer.Filter = offer.Filter.TrimEnd(',');
            //    currentUserProfile.Offers.Add(offer);
            //    _db.Offers.Create(offer);
            //    _db.Save();
            //}

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

        //public ActionResult Edit(int? id = 1)
        //{
        //    Offer offer = _db.Offers.Get(id.ToString());
        //    ApplicationUser user = _db.Users.Get(User.Identity.GetUserId<string>());
        //    OfferViewModel model = new OfferViewModel()
        //    {
        //        Id = offer.Id,
        //        Header = offer.Header,
        //        Discription = offer.Discription,
        //        EndDate = offer.EndDate,
        //        Game = offer.Game,
        //        Price = offer.Price,
        //        StartDate = offer.StartDate,
        //        SteamLogin = offer.SteamLogin,
        //        Filter = offer.Filter

        //    };
        //    if (offer != null && user != null)
        //    {
        //        if (user.UserProfile.Offers.Contains(offer))
        //        {
        //            return View(model);
        //        }

        //    }
        //    return HttpNotFound("You are has no this offer");

        //}

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
