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
        public int pageSize = 4;
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
        public ViewResult Buy(SearchViewModel searchInfo, string[] fi)
        {
            
            var offers = _offerService.GetOffers().Where(o => o.Game.Value == searchInfo.Game && o.State == OfferState.active).ToList();
            if (offers.Any())
            {
                searchInfo.MinGamePrice = offers.Min(o => o.Price);
                searchInfo.MaxGamePrice = offers.Max(o => o.Price);
                searchInfo.PriceFrom = offers.Min(o => o.Price);
                searchInfo.PriceTo = offers.Max(o => o.Price);
            }

            var model = SearchOffers(searchInfo, null);
            
            var game = _gameService.GetGameByValue(searchInfo.Game);
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
            
            model.Games = Mapper.Map<IEnumerable<Game>,IEnumerable<GameViewModel>>(_gameService.GetGames().OrderBy(g => g.Rank));
            return View(model);
        }

        [Authorize]
        public ViewResult Active()
        {
            IEnumerable<Offer> offersActive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.active);
            IEnumerable<Offer> offersInactive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.inactive);
            IEnumerable<Offer> offersClosed = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.closed);
            IEnumerable<OfferViewModel> offerViewModels;
            offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offersActive);
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
            IEnumerable<Offer> offersActive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.active);
            IEnumerable<Offer> offersInactive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.inactive);
            IEnumerable<Offer> offersClosed = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.closed);
            IEnumerable<OfferViewModel> offerViewModels;
            offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offersInactive);
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
            IEnumerable<Offer> offersActive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.active);
            IEnumerable<Offer> offersInactive = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.inactive);
            IEnumerable<Offer> offersClosed = _offerService.GetOffers().Where(m => m.UserProfileId == User.Identity.GetUserId() && m.State == OfferState.closed);
            IEnumerable<OfferViewModel> offerViewModels;
            offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offersClosed);
            OfferListViewModel model = new OfferListViewModel
            {
                Offers = offerViewModels.OrderByDescending(o => o.DateCreated),
                ActiveOffersCount = offersActive.Count(),
                InactiveOffersCount = offersInactive.Count(),
                CloseOffersCount = offersClosed.Count()
            };
            return View(model);
        }
        private OfferListViewModel SearchOffers(SearchViewModel searchInfo,  string[] filters)
        {
            if (filters == null)
            {
                filters = new string[0];
            }
            var s = searchInfo.FilterValues;
            var se = searchInfo.FilterItemValues;
            var filterss = ViewBag.Filters;
            //Thread.Sleep(500);

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
            IEnumerable<Offer> foundOffers = _offerService.SearchOffers(game, sort, ref isOnline, ref searchInDiscription,
                searchString, ref page, pageSize, ref totalItems, ref minGamePrice, ref maxGamePrice, ref priceFrom, ref priceTo,filters);
            if (searchInfo.IsOnline)
            {
                var loggedOnUsers = HttpRuntime.Cache["LoggedInUsers"] as Dictionary<string, DateTime>;
                if (loggedOnUsers != null)
                {
                    foundOffers = foundOffers.Where(o => loggedOnUsers.Any(u => u.Key == o.UserProfile.Name));
                }
            }


            

            IList<SelectListItem> ranks = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Все ранги", Value = "none", Selected = true }
            };
            IList<OfferViewModel> offerList = new List<OfferViewModel>();
            var offerViewModels = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(foundOffers);

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
                    TotalItems = foundOffers.Count()
                }
            };
            model.SearchInfo.SortItems = new List<SelectListItem>
            {
                new SelectListItem { Value = "bestSeller", Text = "Лучшие продавцы" },
                new SelectListItem { Value = "priceDesc", Text = "От дорогих к дешевым" },
                new SelectListItem { Value = "priceAsc", Text = "От дешевых к дорогим" },
                new SelectListItem { Value = "newestOffer", Text = "Самые новые" }
            };

            foreach (var item in model.SearchInfo.SortItems)
            {
                if (item.Value == searchInfo.Sort)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
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
            

            var model = SearchOffers(searchInfo, filters);
            
            return PartialView("List", model);
        }

        public PartialViewResult OfferListInfo(SearchOffersInfoViewModel searchInfo)
        {
            searchInfo.SearchString = searchInfo.SearchString ?? "";
            var offers = _offerService.GetOffers().Where(m => m.UserProfileId == searchInfo.UserId && m.State == OfferState.active);

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
            var userProfile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            if (userProfile != null)
            {
                var appUser = userProfile.ApplicationUser;
                if (appUser != null)
                {
                    if (!(appUser.PhoneNumberConfirmed && appUser.EmailConfirmed))
                    {
                        return View("PhoneNumberRequest");
                    }
                    else
                    {
                        CreateOfferViewModel model = new CreateOfferViewModel();
                        SelectList selectList = new SelectList(_gameService.GetGames());

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

            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateOfferViewModel model, HttpPostedFileBase[] images)
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
                    if (!(appUser.PhoneNumberConfirmed && appUser.EmailConfirmed))
                    {
                        return HttpNotFound("you are not confirmed email or phone number");
                    }
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
            if (modelFilters != null && gameFilters.Count() != 0)
            {
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
            }

           
            foreach (var image in images)
            {
                if (image != null && image.ContentLength <= 1000000 && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                {
                    string extName = System.IO.Path.GetExtension(image.FileName);
                    string fileName = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);

                    // сохраняем файл в папку Files в проекте
                    string fullPath = Server.MapPath("~/Content/Images/Screenshots/" + fileName);
                    string urlPath = Url.Content("~/Content/Images/Screenshots/" + fileName);
                    image.SaveAs(fullPath);
                    offer.ScreenshotPathes.Add(new ScreenshotPath { Value = urlPath } );
                }
                else
                {
                    offer.ScreenshotPathes.Add(new ScreenshotPath { Value = null });
                }
            }

            offer.Game = game;
            offer.UserProfile = _userProfileService.GetUserProfileById(User.Identity.GetUserId());
            offer.DateDeleted = offer.DateCreated.AddDays(offerDays);
            
            _offerService.CreateOffer(offer);
            _offerService.SaveOffer();
            
            offer.JobId = MarketHangfire.SetDeactivateOfferJob(offer.Id, Url.Action("Activate", "Offer", new { id = offer.Id }, protocol: Request.Url.Scheme), TimeSpan.FromDays(30));

            _offerService.SaveOffer();

            return RedirectToAction("Buy");
        }

        public ActionResult Details(int? id = 1)
        {
            if (id != null)
            {
                Offer offer = _offerService.GetOffer(id.Value);
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
                var offer = _offerService.GetOffer(id.Value);

                if (offer.JobId != null)
                {
                    BackgroundJob.Delete(offer.JobId);
                    offer.JobId = null;
                }                
                _offerService.DeactivateOffer(offer, User.Identity.GetUserId());
                _offerService.SaveOffer();
                return View();
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

                    offer.JobId = MarketHangfire.SetDeactivateOfferJob(offer.Id, Url.Action("Activate", "Offer", new { id = offer.Id }, protocol: Request.Url.Scheme), TimeSpan.FromDays(30));
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
            foreach (var game in _gameService.GetGames().OrderBy(g => g.Rank))
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
        [Authorize]
        public ActionResult Edit(EditOfferViewModel model, HttpPostedFileBase[] images)
        {
            //EditOfferViewModel model = new EditOfferViewModel();
            
            if (!ModelState.IsValid)
            {
                return RedirectToAction("View" , new { id = model.Id });
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
                            string extName = System.IO.Path.GetExtension(images[i].FileName);
                            string fileName = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);

                            // сохраняем файл в папку Files в проекте
                            string fullPath = Server.MapPath("~/Content/Images/Screenshots/" + fileName);
                            string urlPath = Url.Content("~/Content/Images/Screenshots/" + fileName);
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
            Offer offer = _offerService.GetOffers().LastOrDefault(x => x.AccountLogin == steamLogin);

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
