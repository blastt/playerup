using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Market.Web.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IGameService _gameService;
        private readonly IFilterService _filterService;
        private readonly IFilterItemService _filterItemService;
        public int pageSize = 7;
        public OfferController(IOfferService offerService, IGameService gameService, IFilterService filterService, IFilterItemService filterItemService)
        {
            _offerService = offerService;
            _gameService = gameService;
            _filterService = filterService;
            _filterItemService = filterItemService;
        }

        // GET: Offer
        public ViewResult List()
        {
            decimal minPrice = 0;
            decimal maxPrice = 0;
            IEnumerable<Offer> offers = _offerService.GetOffers();


            if (offers.Count() != 0 && offers != null)
            {
                offers = offers.Where(m => m.Game.Value == "csgo");
                minPrice = offers.Min(m => m.Price);
                maxPrice = offers.Max(m => m.Price);
            }
            IList<SelectListItem> ranks = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Все ранги", Value = "none", Selected = true }
            };
            foreach (var rank in (Dictionary<string, string>)(GetFiltersJson("csgo").Data))
            {
                ranks.Add(new SelectListItem() { Text = rank.Value, Value = rank.Key });
            }
            IList<OfferViewModel> offerList = new List<OfferViewModel>();
            foreach (var offer in _offerService.GetOffers().Where(m => m.Game.Value == "csgo"))
            {
                offerList.Add(Mapper.Map<Offer, OfferViewModel>(offer));
            }
            var model = new OfferListViewModel()
            {
                Filters = _filterService.GetFilters().Where(m => m.Game.Value == "csgo"),
                Game = _gameService.GetGameByValue("csgo"),
                Offers = offerList
            };
            
            

            return View(model);
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

            // Сделать проверку на налл
            var json = new JavaScriptSerializer().Serialize(_gameService.GetGameByValue(game).Filters);
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
